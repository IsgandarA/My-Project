using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotorblade : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, speed*3, 0.0f, Space.Self);
    }
}
