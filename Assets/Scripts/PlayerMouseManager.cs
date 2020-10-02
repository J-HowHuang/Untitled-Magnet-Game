using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject hud;
    public Texture2D cursor;
    public Texture2D cursorActivate;
    public float cursorRadius;
    public float cameraMovementSpeed;
    public float cameraEdgeSize;
    GameObject hoverObject;
    RaycastHit2D hit;
    LineRenderer lr;
    CameraFollow mainCameraFollow;
    Vector3 hudOffset = new Vector3(0, 0, -1);
    private void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        lr = hud.GetComponent<LineRenderer>();
        mainCameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Update()
    {
        hoverDetect();
        clickDetect();
        edgeMovingDetect();
    }

    void hoverDetect()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        hit = Physics2D.CircleCast(mousePos2D, cursorRadius, Vector2.zero);
        if (hit)
        {
            hoverObject = hit.collider.gameObject;
            displayAttractTrack();
        }
        else
        {
            hoverObject = null;
            cancelAttractTrack();
        }
    }

    void clickDetect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorActivate, Vector2.zero, CursorMode.ForceSoftware);
            if (hoverObject != null)
            {
                if (hoverObject.GetComponent<Attractable>() != null)
                {
                    hoverObject.GetComponent<Attractable>().startBeingAttracted(player);
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    void edgeMovingDetect()
    {
        if (Input.mousePosition.x > Screen.width - cameraEdgeSize){
            mainCameraFollow.movePosition(Time.deltaTime * cameraMovementSpeed * new Vector3(1,0,0));
        }
        if (Input.mousePosition.x < cameraEdgeSize){
            mainCameraFollow.movePosition(Time.deltaTime * cameraMovementSpeed * new Vector3(-1,0,0));
        }
        if (Input.mousePosition.y > Screen.height - cameraEdgeSize){
            mainCameraFollow.movePosition(Time.deltaTime * cameraMovementSpeed * new Vector3(0,1,0));
        }
        if (Input.mousePosition.y < cameraEdgeSize){
            mainCameraFollow.movePosition(Time.deltaTime * cameraMovementSpeed * new Vector3(0,-1,0));
        }
    }
    void displayAttractTrack()
    {
        if (lr == null)
        {
            lr = hud.GetComponent<LineRenderer>();
        }
        lr.enabled = true;
        lr.positionCount = 2;
        lr.SetPosition(0, hoverObject.transform.position + hudOffset);
        lr.SetPosition(1, player.transform.position + hudOffset);

    }
    void cancelAttractTrack()
    {
        lr.enabled = false;
    }
}
