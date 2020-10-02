using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float magneticForce;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool beingAttracted = false;
    float dashDurationLeft = 0;
    GameObject attractor;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (dashDurationLeft > 0)
        {
            beingAttracted = false;
            dashDurationLeft -= Time.deltaTime;
            if (dashDurationLeft <= 0)
            {
                rb.velocity = Vector2.zero;
            }
        }
        else if (beingAttracted)
        {
            Vector3 vector_to_attractor = attractor.transform.position - gameObject.transform.position;
            Vector3 movement = vector_to_attractor / vector_to_attractor.magnitude;
            rb.MovePosition(
                transform.position + transform.TransformDirection(movement) * Time.deltaTime * magneticForce
            );
        }
        else {
            moveDetect();
        }
    }
    void moveDetect()
    {
        if (Input.GetKey("s") && Input.GetKey("d"))
        {
            WalkDirection(315);
        }
        else if (Input.GetKey("s") && Input.GetKey("a"))
        {
            WalkDirection(225);
        }
        else if (Input.GetKey("a") && Input.GetKey("w"))
        {
            WalkDirection(135);
        }
        else if (Input.GetKey("w") && Input.GetKey("d"))
        {
            WalkDirection(45);
        }
        else if (Input.GetKey("a"))
        {
            WalkDirection(180);
        }
        else if (Input.GetKey("d"))
        {
            WalkDirection(0);
        }
        else if (Input.GetKey("w"))
        {
            WalkDirection(90);
        }
        else if (Input.GetKey("s"))
        {
            WalkDirection(270);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }
    void WalkDirection(float angle)
    {
        animator.SetBool("moving", true);
        animator.SetFloat("moveX", Mathf.Cos(angle * Mathf.PI / 180));
        animator.SetFloat("moveY", Mathf.Sin(angle * Mathf.PI / 180));
        rb.MovePosition(
            transform.position + movementSpeed * new Vector3(Mathf.Cos(angle * Mathf.PI / 180), Mathf.Sin(angle * Mathf.PI / 180), 0)
        );
    }

    public void attractedTo(GameObject obj)
    {
        attractor = obj;
        beingAttracted = true;
    }

    public void dash(float duration, Vector2 direction)
    {
        dashDurationLeft = duration;
        rb.velocity = direction / duration;
    }
    void OnCollisionStay2D(Collision2D other)
    {
        beingAttracted = false;
    }
}
