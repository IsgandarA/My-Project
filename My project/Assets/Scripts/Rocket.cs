using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Missile
{
    private Turret turret;
    [SerializeField] AudioClip launch;
    [SerializeField] ParticleSystem fire;
    private GameObject rocketLock;
    private AudioSource audio;
    //private Vector3 aim;
    //private Vector3 startPos;
    //private float startTime;
    //private float journeyLength;
    private bool playerRocket;

    //protected override void Update()
    //{
    //    if (transform.position == aim)
    //    {
    //        StartCoroutine("SelfDestruct");
    //    }
    //}
    void OnEnable()
    {
        speedVar = 120;
        audio = GetComponent<AudioSource>();
        audio.volume = 0.2f;
        audio.PlayOneShot(launch, 0.2f);
        fire.gameObject.SetActive(true);
        startPos = transform.position;
        startTime = Time.time;
        if (transform.root.CompareTag("Player"))
        {
            playerRocket = true;
            turret = GameObject.Find("Turret").GetComponent<Turret>();
            if (turret.lockedOn == true)
            {
                rocketLock = turret.rocketLock;
            }
            else
            {
                rocketLock = null;
            }
            aim = turret.aim;
            transform.rotation = turret.transform.rotation;
        }
        else
        {
            
            aim = GameObject.Find("Player").transform.position;
            transform.LookAt(aim);
        }
        journeyLength = Vector3.Distance(startPos, aim);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (playerRocket && other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("collision");
            other.GetComponent<Enemy>().Health = 0;
            Destroy(gameObject);
        }
        else if (!playerRocket && other.gameObject.CompareTag("Player"))
        {

            Player.Instance.Health -= 2;
            Debug.Log(Player.Instance.Health);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    protected override void FixedUpdate()
    { 
        if (playerRocket&&rocketLock!=null)
        {
            if (transform.position.z < rocketLock.transform.position.z)
            {
                aim = rocketLock.transform.position;
                transform.LookAt(aim);
            }
            else
            {
                aim = new Vector3(rocketLock.transform.position.x, rocketLock.transform.position.y, 1000);
            }
        }
        else if (!playerRocket)
        {
            if (transform.position.z - Player.Instance.transform.position.z >60)
            {
                aim = Player.Instance.transform.position;
                transform.LookAt(aim);
            }
            else
            {
                
            }
        }
        base.FixedUpdate();
    }
    protected override IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(.05f);
        GameManager.Instance.Explosion(transform.position, false);
        Destroy(gameObject);
    }
}
