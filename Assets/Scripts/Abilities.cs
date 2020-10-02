using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public GameObject player;
    public float punchRange;
    public GameObject hud;
    LineRenderer lr;
    RaycastHit2D[] hit;
    Vector3 hudOffset = new Vector3(0, 0, -1);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            displayAbilityRange(player.transform.position, punchRange);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            lr.enabled = false;
            punch();
        }
    }

    void displayAbilityRange(Vector2 center, float range)
    {
        int segments = 16;
        lr = hud.GetComponent<LineRenderer>();
        lr.enabled = true;
        lr.positionCount = segments + 1;

        float x;
        float y;
        float z = 0;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = center.x + Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            y = center.y + Mathf.Cos(Mathf.Deg2Rad * angle) * range;

            lr.SetPosition(i, new Vector3(x, y, z) + hudOffset);

            angle += (360f / segments);
        }
    }
    void punch()
    {
        Vector3 playerPos = player.transform.position;
        Vector2 playerPos2D = new Vector2(playerPos.x, playerPos.y);
        float knockBackTime = 0.2f;
        float knockBackDistance = 3;

        hit = Physics2D.CircleCastAll(playerPos2D, punchRange, Vector2.zero);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.GetComponent<PlayerMovement>() != null)
            {
                if (hit[i].collider.gameObject == player)
                {
                    continue;
                }
                Vector3 direction = hit[i].collider.gameObject.transform.position - playerPos;
                direction = direction / direction.magnitude;
                hit[i].collider.gameObject.GetComponent<PlayerMovement>().dash(knockBackTime, knockBackDistance * direction);
            }
            else if (hit[i].collider.gameObject.GetComponent<Attractable>() != null)
            {
                Vector3 direction = hit[i].collider.gameObject.transform.position - playerPos;
                direction = direction / direction.magnitude;
                hit[i].collider.gameObject.GetComponent<Attractable>().dash(knockBackTime, knockBackDistance * direction);
            }
        }
    }
}
