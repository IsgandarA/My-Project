using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private Quaternion rotation;
    [SerializeField] GameObject player;
    float x;
    float y;
    float rotX;
    float rotY;
    
    // Start is called before the first frame update
    void Start()
    {
       transform.rotation = Quaternion.Euler(-20, 0, 0);
       transform.position = new Vector3(0, player.transform.position.y, -200);
    }

    // Update is called once per frame
    void Update()
    { 
        if (player != null)
        {

            //if (player.transform.position.x == 0)
            //{
            //    x = 0;
            //}
            //if (player.transform.position.x < -10)
            //{
            //    x = player.transform.position.x+10;
            //}
            //if (player.transform.position.x > 10)
            //{
            //    x = player.transform.position.x-10;
            //}
            x = player.transform.position.x - player.transform.position.x / 20;
            rotY = -(player.transform.position.x) / 25;
            y = player.transform.position.y +20 - (player.transform.position.y - 250) / 20;
            rotX = (player.transform.position.y - 250)/30;
            transform.rotation = Quaternion.Euler(rotX, rotY, 0);
            transform.position = new Vector3(x, y, -200);

        }
    }
}
