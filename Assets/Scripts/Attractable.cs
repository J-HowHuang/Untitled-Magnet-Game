using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
    bool beingAttracted = false;
    float magneticForce;
    public bool isFixed;
    GameObject attractor;
    Rigidbody2D rb;
    float dashDurationLeft = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void startBeingAttracted(GameObject attractTo)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        attractor = attractTo;
        beingAttracted = true;
        magneticForce = attractor.GetComponent<PlayerMovement>().magneticForce;
    }

    void endBeingAttrated()
    {
        rb.velocity = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        beingAttracted = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (dashDurationLeft > 0)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            beingAttracted = false;
            dashDurationLeft -= Time.deltaTime;
            if (dashDurationLeft <= 0)
            {
                rb.velocity = Vector2.zero;
            }
        }
        if (beingAttracted && !isFixed)
        {
            Vector3 vector_to_attractor = attractor.transform.position - gameObject.transform.position;
            Vector3 movement = vector_to_attractor / vector_to_attractor.magnitude;
            transform.position += transform.TransformDirection(movement) * Time.deltaTime * magneticForce;
        }
        else if (beingAttracted && isFixed)
        {
            attractor.GetComponent<PlayerMovement>().attractedTo(gameObject);
            endBeingAttrated();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        endBeingAttrated();
        Debug.Log(gameObject.name + " collide " + other.collider.name);
    }


    public void dash(float duration, Vector2 direction)
    {
        dashDurationLeft = duration;
        rb.velocity = direction / duration;
    }
}
