using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buiilding : MonoBehaviour
{
    Vector3 startPos;
    float speedMod = 1;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawner.Instance.level == 3)
        {
            speedMod = 2;
        }
        else if (Spawner.Instance.level >= 4)
        {
            speedMod = 2.5f;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * 900*speedMod);
        
        if (transform.position.z < -100)
        {
            Destroy(gameObject);
        }
    }

}
