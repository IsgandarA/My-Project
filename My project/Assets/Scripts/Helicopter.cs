using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Helicopter : MonoBehaviour
{
    protected float[] bounds = new float[2];
    protected float vertical { get; set; }
    protected int health;
    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            if (value.GetType() == typeof(int))
            {
                health = value;
            }
        }
        }
    protected float horizontal { get; set; }
    protected float boundsBounce = 0.05f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        bounds[0] = vertical;
        bounds[1] = horizontal;

}

// Update is called once per frame
    protected virtual void FixedUpdate()
    {

    }
    protected virtual void Attack(int i)
    {

    }
    protected abstract void Move();

    protected void Bounds(float[] bounds)
    {
        if (transform.position.x > bounds[1])
        {
            transform.position += new Vector3(-boundsBounce, 0, 0); 
        }
        if (transform.position.x < -1 * bounds[1])
        {
            transform.position += new Vector3(boundsBounce, 0, 0);
        }
        if (transform.position.y > bounds[0])
        {
            transform.position += new Vector3(0, -boundsBounce, 0);
        }
        if (transform.position.y < -1 * bounds[0])
        {
            transform.position += new Vector3(0, boundsBounce, 0);
        }


    }


}
