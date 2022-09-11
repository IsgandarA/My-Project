using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Enemy : Helicopter
{
    protected Vector3 startPos;
    protected float startTime;
    [SerializeField] protected AudioClip bulletHit1;
    [SerializeField] protected AudioClip bulletHit2;
    [SerializeField] protected GameObject[] rocketsSlots;
    [SerializeField] protected GameObject[] rockets;
    [SerializeField] protected GameObject rocketPrefab;
    [SerializeField] protected GameObject sparks;


    protected AudioSource audio;
    protected int bulletHitSoundCont =1;
    protected int x = 0;
    protected float y = 0;
    protected bool cooldown = false;
    protected bool peak= false;
    protected bool peak2 = false;

    protected float speedmod = 90f;
    protected float xOffset;
    protected float yOffset;
    protected float xPos;
    protected float yPos;
    protected float xMax;
    protected float xMin;




    protected IEnumerator Attack() //ABSTRACTION
                                   //There are obviously plenty of functions in the code of the project, but as an example, this coroutine shoots and respawns
                                   //the enemy rockets from all available slots (2 for reg enemies, 4 for the boss).
    {
        GameObject rocket = rockets[x];
        yield return new WaitForSeconds(2);
        rocket.GetComponent<Rocket>().enabled = true;
            rocket.transform.parent = null;
            rockets[x] = null;
            yield return new WaitForSeconds(3);
            //      if (rockets[System.Array.IndexOf(rocketsSlots, rocket)] == null)
            //      System.Array.IndexOf(rocketsSlots, rocket)
            rockets[x] = Instantiate(rocketPrefab, rocketsSlots[x].transform.position, rocketsSlots[x].transform.rotation);
            rockets[x].transform.parent = rocketsSlots[x].transform;
            x += 1;
            if (x == rocketsSlots.Length)
            {
                x = 0;
            }
            StartCoroutine("Attack");
        }
    
    protected virtual void Awake()

    {
        xMin = transform.position.x;
        xOffset = transform.position.x;
        yOffset = transform.position.y;
        StartCoroutine("Attack");
        audio = GetComponent<AudioSource>();
        health = 7;
        startPos = transform.position;
        startTime = Time.time;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        //healthSlider.transform.position = new Vector3(transform.position.x, transform.position.y + 15, transform.position.z);
        //healthSlider.value = health;
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
            if (transform.position.z - Player.Instance.transform.position.z > 300)
            {
                
                float distCovered = (Time.time - startTime) * 70;
                float fractionOfJourney = distCovered / Vector3.Distance(startPos, new Vector3(transform.position.x, transform.position.y, Player.Instance.transform.position.z + 300));
                transform.position = Vector3.Lerp(startPos, new Vector3(transform.position.x, transform.position.y, Player.Instance.transform.position.z + 300), fractionOfJourney);
            }

            if (Spawner.Instance.level < 3)
            {
                    if (transform.position.x < 200 && !peak)
                    {
                        xPos = transform.position.x + Time.deltaTime * speedmod * (Spawner.Instance.level * .75f);
                        yPos = transform.position.y;
                    }
                    if (transform.position.x >= 200)
                    {
                        peak = true;
                    }
                    if (transform.position.x > -200 && peak)
                    {
                        xPos= transform.position.x - Time.deltaTime * speedmod * (Spawner.Instance.level * .75f);
                        yPos = transform.position.y;
                    }
                    if (transform.position.x <= -200)
                    {
                        peak = false;
                    }
                transform.position = new Vector3(xPos, yPos, 200);
            }
                if (Spawner.Instance.level > 2 && Spawner.Instance.level < 5)
                {
                    if (transform.position.x < 250 && !peak)
                    {
                        xPos = transform.position.x + Time.deltaTime * speedmod * (Spawner.Instance.level / 2.5f);
                        yPos = transform.position.y + Time.deltaTime * speedmod * (Spawner.Instance.level / 6f);
                    }
                    if (transform.position.x >= 250)
                    {
                        peak = true;
                    }
                    if (transform.position.x > -250 && peak)
                    {
                        xPos = transform.position.x - Time.deltaTime * speedmod * (Spawner.Instance.level / 2.5f);
                        yPos = transform.position.y - Time.deltaTime * speedmod * (Spawner.Instance.level / 6f);
                    }
                    if (transform.position.x <= -250)
                    {
                        peak = false;

                    }
                    transform.position = new Vector3(xPos, yPos, 200);
                }
                if (Spawner.Instance.level > 4)
                {
                    if (!peak)
                    {
                        yPos = yPos + Time.deltaTime * speedmod;
                    }
                    if (yPos >= yOffset + 79.9f)
                    {
                        peak = true;
                }
                    if (peak)
                    {
                        yPos = yPos - Time.deltaTime * speedmod;
                    }
                    if (yPos <= yOffset - 79.9f)
                    {
                        peak = false;
                    }
                Debug.Log(xMax +", "+ xMin);

                xPos = xOffset + Mathf.Abs(Mathf.Sqrt(Mathf.Abs(6400 - Mathf.Pow(yPos - yOffset, 2))));
                //if (xPos > xMax)
                //{
                //    xMax = xPos;
                //}
                //if (xPos < xMin)
                //{
                //    xMin = xPos;
                //}
                if (!peak)
                {
                    xPos = xOffset + Mathf.Sqrt(Mathf.Abs(6400 - Mathf.Pow(yPos - yOffset, 2)));
                }
                if (peak)
                {
                    xPos = xOffset - Mathf.Sqrt(Mathf.Abs(6400 - Mathf.Pow(yPos - yOffset, 2)));
                }
                
                transform.position = new Vector3(xPos, yPos, 200);
                }
            }
        
        catch (MissingReferenceException e)
        {
            Destroy(gameObject);
        }
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if  (other.gameObject.CompareTag("Bullet"))
        {
            Instantiate(sparks, other.transform.position, sparks.transform.rotation);
            
            if (bulletHitSoundCont == 1)
            {
                audio.PlayOneShot(bulletHit1, 1f);
                bulletHitSoundCont = 2;
            }
            else
            {
                audio.PlayOneShot(bulletHit2, 1f);
                bulletHitSoundCont = 1;
            }

            health -= 1;
            Destroy(other);
        }

    }


    protected override void Move()
    {
        
    }
    
}
