using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Helicopter
{
    Vector3 startPos;
    float startTime;
    [SerializeField] AudioClip bulletHit1;
    [SerializeField] AudioClip bulletHit2;
    [SerializeField] GameObject[] rocketsSlots;
    [SerializeField] GameObject[] rockets;
    [SerializeField] GameObject rocket;
    [SerializeField] GameObject sparks;
    private AudioSource audio;
    private int bulletHitSoundCont=1;
    private int x;
    private float y = 0;
    private bool cooldown;
    private bool peak;
    private int speedmod = 120;
    private float xOffset;
    private float yOffset;


    // Start is called before the first frame update
    void Attack(int x)
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
        xOffset = transform.position.x - 30;
        yOffset = transform.position.y;
        StartCoroutine("Attacks");
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
            try
            {
                Title.Instance.score += 1;
                Title.Instance.highScore.text = "Score: " + Title.Instance.score;
            }
            catch (NullReferenceException e)
            {

            }
            Destroy(gameObject);
        }
        try
        {
            if (transform.position.z - Player.Instance.transform.position.z > 250)
            {
                
                float distCovered = (Time.time - startTime) * 70;
                float fractionOfJourney = distCovered / Vector3.Distance(startPos, new Vector3(transform.position.x, transform.position.y, Player.Instance.transform.position.z + 250));
                transform.position = Vector3.Lerp(startPos, new Vector3(transform.position.x, transform.position.y, Player.Instance.transform.position.z + 250), fractionOfJourney);
            }
            if (transform.position.z - Player.Instance.transform.position.z == 250)
            {
                //if (transform.position.y >= yOffset + 29.5f)
                //{
                //    peak = true;
                //}
                //if (peak)
                //{
                //    if (transform.position.y > yOffset + 29.5f && transform.position.y < yOffset + 30)
                //    {
                //        transform.position = new Vector3(xOffset, transform.position.y - Time.deltaTime * speedmod, Player.Instance.transform.position.z + 130);
                //    }
                //    else
                //    {
                //        transform.position = new Vector3(xOffset - Mathf.Sqrt(900 - Mathf.Pow(transform.position.y - yOffset, 2)), transform.position.y - Time.deltaTime * speedmod, Player.Instance.transform.position.z + 130);
                //    }
                //}
                //if (transform.position.y <= yOffset-29.5f)
                //{
                //    peak = false;
                //}
                //if (!peak)
                //{
                //    if (transform.position.y < yOffset - 29.5f && transform.position.y > yOffset - 30)
                //    {
                //        transform.position = new Vector3(xOffset, transform.position.y + Time.deltaTime * speedmod, Player.Instance.transform.position.z + 130);
                //    }
                //    else
                //    {
                //        transform.position = new Vector3(xOffset + Mathf.Sqrt(900 - Mathf.Pow(transform.position.y - yOffset, 2)), transform.position.y + Time.deltaTime * speedmod, Player.Instance.transform.position.z + 130);

                //    }
                //}
                if (transform.position.x < 200 &&!peak)
                {
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * speedmod, transform.position.y, transform.position.z);
                }
                if (transform.position.x >= 200)
                {
                    peak = true;
                }
                if (transform.position.x > -200 && peak)
                {
                    transform.position = new Vector3(transform.position.x - Time.deltaTime * speedmod, transform.position.y, transform.position.z);
                }
                if (transform.position.x <= -200)
                {
                    peak = false;
                }

            }
        }
        catch (MissingReferenceException e)
        {
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
        }
    }

    IEnumerator Attacks()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
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
