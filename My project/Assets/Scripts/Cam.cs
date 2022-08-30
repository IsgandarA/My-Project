using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private Quaternion rotation;
    [SerializeField] GameObject player;
    Vector3 lastpos;
    // Start is called before the first frame update
    void Start()
    {
       transform.rotation = Quaternion.Euler(10, 0, 0);
       transform.position = new Vector3(0, player.transform.position.y +20, -170);
    }

    // Update is called once per frame
    void Update()
    { 
        if (player != null)
        {
            if (player.transform.position.x >= -30 || player.transform.position.x <= 30)
            {
                transform.position = new Vector3(0, player.transform.position.y + 20, -170);
            }
            if (player.transform.position.x < -30)
            {
                transform.position = new Vector3(player.transform.position.x+30, player.transform.position.y + 20, - 170);
            }
            if (player.transform.position.x > 30)
            {
                transform.position = new Vector3(player.transform.position.x-30, player.transform.position.y + 20, -170);
            }

        }
    }
}
