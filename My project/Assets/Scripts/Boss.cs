using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    private bool dodgeCooldown;
    private Vector3 dodgeStartPos;
// Start is called before the first frame update
    protected override void Awake()
    {

        StartCoroutine("Attack");
        health = 20;
        x = 0;
        cooldown = false;
        Title.Instance.bossHealth.gameObject.SetActive(true);
        audio = GetComponent<AudioSource>();
        base.Awake();
    }

        // Update is called once per frame
        void Update()
    {
        Title.Instance.bossHealth.value = health;
        if (health == 0)
        {
            Title.Instance.StartCoroutine("Victory");
            Destroy(gameObject);
        }
        base.Update();

        if (Player.Instance.lockedRocketBool == false)
        {
            dodgeStartPos = transform.position;
            dodgeCooldown = false;

        }
        if (Player.Instance.lockedRocketBool == true)
        { 
            dodgeCooldown = true;
            if (dodgeStartPos.x >= 0)
            {
                Vector3.Lerp(dodgeStartPos, new Vector3(startPos.x - 200, startPos.y, startPos.z), 0.5f);

            }
            else if (transform.position.x < 0)
            {
                Vector3.Lerp(dodgeStartPos, new Vector3(startPos.x + 200, startPos.y, startPos.z), 0.5f);
            }
        }


    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
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
        if (other.gameObject.CompareTag("PlayerRocket"))
        {
            health -= 5;
            GameManager.Instance.Explosion(other.transform.position, true);
            Destroy(other.gameObject);
        }
    }

}
