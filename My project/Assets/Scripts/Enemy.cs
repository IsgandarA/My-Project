using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Helicopter
{
    Vector3 startPos;
    float startTime;
    [SerializeField] AudioClip bulletHit1;
    [SerializeField] AudioClip bulletHit2;
    [SerializeField] GameObject[] rocketsSlots;
    [SerializeField] GameObject[] rockets;
    [SerializeField] GameObject rocket;
    private AudioSource audio;
    [SerializeField] GameObject sparks;
    private int bulletHitSoundCont=1;
    private int x;
    private bool cooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void Attack(int x)
    {
        GameObject aRocket = rockets[x];
        {
            if (aRocket != null)
            {
                aRocket.GetComponent<Rocket>().enabled = true;
                aRocket.transform.parent = null;
                rockets[x] = null;
                StartCoroutine(RocketRespawn(rocketsSlots[x]));
            }
            else
            {
                x += 1;
                Attack(x);
                cooldown = true;
            }
            if (x == 2)
            {
                x = 0;
            }
        }
        IEnumerator RocketRespawn(GameObject slot)
        {
            yield return new WaitForSeconds(5);
            cooldown = false;
            rockets[System.Array.IndexOf(rocketsSlots, slot)] = Instantiate(rocket, slot.transform.position, slot.transform.rotation);
            rockets[System.Array.IndexOf(rocketsSlots, slot)].transform.parent = slot.transform;
        }
    }
    private void Awake()

    {
        x = 0;
        StartCoroutine("Attacks");
        vertical = 200;
        horizontal = 200;
        bounds[0] = vertical;
        bounds[1] = horizontal;
        audio = GetComponent<AudioSource>();
        health = 7;
        startPos = transform.position;
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            GameManager.Instance.Explosion(transform.position, true);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if  (other.gameObject.CompareTag("Bullet"))
        {
            Instantiate(sparks, other.transform.position, sparks.transform.rotation);
            
            if (bulletHitSoundCont == 1)
            {
                audio.PlayOneShot(bulletHit1, .3f);
                bulletHitSoundCont = 2;
            }
            else
            {
                audio.PlayOneShot(bulletHit2, .3f);
                bulletHitSoundCont = 1;
            }

            health -= 1;
            Destroy(other);
            Debug.Log(health);
        }
    }
    protected override void FixedUpdate()
    {
        if (transform.position.z - Player.Instance.transform.position.z > 15)
        {
            float distCovered = (Time.time - startTime)*5;
            float fractionOfJourney = distCovered / Vector3.Distance(startPos, new Vector3(transform.position.x, transform.position.y, Player.Instance.transform.position.z + 30));
            transform.position = Vector3.Lerp(startPos, new Vector3(transform.position.x, transform.position.y, Player.Instance.transform.position.z + 30), fractionOfJourney);
        }

    }
    IEnumerator Attacks()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!cooldown)
            {
                Attack(x);
            }
            
        }
    }
    protected override void Move()
    {
        
    }
    
}
