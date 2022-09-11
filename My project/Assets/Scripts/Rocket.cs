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
    private bool enemyRocket;


    //protected override void Update()
    //{
    //    if (transform.position == aim)
    //    {
    //        StartCoroutine("SelfDestruct");
    //    }
    //}

    public void OnEnable()
    {

        if (transform.root.CompareTag("Player"))
        {
            playerRocket = true;
            speedVar = 160;
            //    tag = "PlayerRocket";
            turret = GameObject.Find("Turret").GetComponent<Turret>();
        }
        else 
        {
            enemyRocket = true;
            speedVar = 120;
            //    tag = "EnemyRocket";
        }
        
        audio = GetComponent<AudioSource>();
        audio.volume = 0.2f;
        audio.PlayOneShot(launch, 0.2f);
        fire.gameObject.SetActive(true);
        startPos = transform.position;
        startTime = Time.time;
        if (playerRocket)
        {

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
        else if (enemyRocket)
        {
            aim = GameObject.Find("Player").transform.position;
            transform.LookAt(aim);
        }
        journeyLength = Vector3.Distance(startPos, aim);

    }
    private void OnTriggerEnter(Collider other)
    {
         if (!playerRocket && other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Explosion(other.transform.position, true);
            Player.Instance.Health -= 2;
            Debug.Log(Player.Instance.Health);
            Destroy(gameObject);
        }
        else if (playerRocket && other.gameObject.CompareTag("Enemy")|| playerRocket && other.gameObject.CompareTag("EnemyRocket"))
        {
            
            GameManager.Instance.Explosion(other.transform.position, true);
            Destroy(gameObject);
            Destroy(other.gameObject.transform.root.gameObject);
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (enemyRocket && collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("shotDown");
            GameManager.Instance.Explosion(transform.position, false);
            StartCoroutine(SelfDestruct());
        }
    }
    // Update is called once per frame
    protected override void FixedUpdate()
    { 
        if (playerRocket)
        {
            if (rocketLock != null && Spawner.Instance.level<4)
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
            if (rocketLock != null && Spawner.Instance.level >= 4)
            {
                if (transform.position.z < rocketLock.transform.position.z-50)
                {
                    aim = rocketLock.transform.position;
                    transform.LookAt(aim);
                }
                else
                {
                    aim = new Vector3(rocketLock.transform.position.x, rocketLock.transform.position.y, 1000);
                }

            }
            base.FixedUpdate();
        }
       

        else if (enemyRocket)
        {
            if (transform.position.z - Player.Instance.transform.position.z >60)
            {
                aim = Player.Instance.transform.position;
                transform.LookAt(aim);
                
            }
            else
            {
                
            }
            base.FixedUpdate();
        }

    }

}
