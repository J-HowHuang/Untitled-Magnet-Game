using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public void setPosition(Vector3 pos){
        pos.z = transform.position.z;
        transform.position = pos;
    }

    public void movePosition(Vector3 movement){
        transform.position += movement;
    }
    private void Update() {
        if(Input.GetKey(KeyCode.Space)){
            setPosition(player.transform.position);
        }
    }
}
