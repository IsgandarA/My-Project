using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Missile : MonoBehaviour
{
    protected Vector3 aim;
    protected Vector3 startPos;
    protected float startTime;
    protected float journeyLength;
    protected float speedVar;
    protected int bounds = 1000;

    virtual protected void FixedUpdate()
    {

        float distCovered = (Time.time - startTime) * speedVar;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPos, aim, fractionOfJourney);
        if (transform.position.z > bounds | transform.position.y > bounds | transform.position.z < -bounds | transform.position.y < -bounds)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void Update()
    {
        if (transform.position == aim)
        {
            StartCoroutine("SelfDestruct");
        }
    }
    virtual protected IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(.05f);
        Destroy(gameObject);
    }
}
