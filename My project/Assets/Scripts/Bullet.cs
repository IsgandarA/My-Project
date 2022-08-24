using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Turret turret;
    private Vector3 aim;
    private Vector3 startPos;
    private float startTime;
    private float journeyLength;
    // Start is called before the first frame update
    void Awake()
    {
        turret = GameObject.Find("Turret").GetComponent<Turret>();
        aim = turret.look;
        startPos = transform.position;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPos,aim);
    }

    void FixedUpdate()
    {
        float distCovered = (Time.time - startTime) *120;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPos, aim, fractionOfJourney);
        if (transform.position == aim)
        {
            StartCoroutine("SelfDestruct");
        }
        if (transform.position.z > 250 | transform.position.y > 250 | transform.position.z < -250 | transform.position.y < -250)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
