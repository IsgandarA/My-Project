using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buiilding : MonoBehaviour
{
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * 400);
        if (transform.position.z < -150)
        {
            Destroy(gameObject);
        }
    }
}
