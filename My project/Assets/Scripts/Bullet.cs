using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Missile //POLYMORPHISM
    //Missile is a class that describes the movement of a projectile and instructs it to self destruct as appropriate. Bullet is a simple modification.
{

    public Turret turret;


    void Awake()
    {
        turret = GameObject.Find("Turret").GetComponent<Turret>();
        aim = turret.look;
        startPos = transform.position;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPos,aim);
        speedVar = 2500;
        
    }
    //protected override void Update()
    //{
    //    if (transform.position == aim)
    //    {
    //        StartCoroutine("SelfDestruct");
    //    }
    //}
    //protected override void FixedUpdate()
    //{
    //    float distCovered = (Time.time - startTime) * 130;
    //    float fractionOfJourney = distCovered / journeyLength;
    //    transform.position = Vector3.Lerp(startPos, aim, fractionOfJourney);

    //    if (transform.position.z > bounds | transform.position.y > bounds | transform.position.z < -bounds | transform.position.y < -bounds)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    //protected override IEnumerator SelfDestruct()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    Destroy(gameObject);
    //}
}
