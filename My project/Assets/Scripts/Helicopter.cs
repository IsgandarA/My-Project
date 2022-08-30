using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Helicopter : MonoBehaviour
{

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

    // Start is called before the first frame update
    protected virtual void Start()
    {


}

// Update is called once per frame

    protected abstract void Move();



}
