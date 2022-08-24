using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private Quaternion rotation;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
       transform.rotation = Quaternion.Euler(30, 0, 0);
    }

    // Update is called once per frame
    void Update()
    { 
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 10, player.transform.position.z - 7);

        }
    }
}
