using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Helicopter : MonoBehaviour
{


    protected int health;
    public int Health //ENCAPSULATION
        //Helicopted is the base class of the player, normal enemies and the boss. The player is a static singleton whose health can be edited by other scripts.
        //Using getters and setters the variable is safeguarded against potential misuse.
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
