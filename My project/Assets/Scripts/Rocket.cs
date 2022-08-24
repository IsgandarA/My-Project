using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Turret turret;
    [SerializeField] AudioClip launch;
    [SerializeField] ParticleSystem fire;

    private AudioSource audio;
    private Vector3 aim;
    private Vector3 startPos;
    private float startTime;
    private float journeyLength;
    private bool playerRocket;
    // Start is called before the first frame update
    private void Update()
    {
        if (transform.position == aim)
        {
            StartCoroutine("SelfDestruct");
        }
    }
    void OnEnable()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = 0.2f;
        audio.PlayOneShot(launch, 0.2f);
        if (transform.root.CompareTag("Player"))
        {
            playerRocket = true;
            turret = GameObject.Find("Turret").GetComponent<Turret>();
            aim = turret.look;
            transform.rotation = turret.transform.rotation;
            startPos = transform.position;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPos, aim);
        }
        else
        {
            
            aim = GameObject.Find("Player").transform.position;
            transform.LookAt(aim);
            startPos = transform.position;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPos, aim);
        }
        
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
    void FixedUpdate()
    {
        fire.gameObject.SetActive(true);
        float distCovered = (Time.time - startTime) * 60;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPos, aim, fractionOfJourney);
        if (transform.position.z > 200 | transform.position.y > 200 | transform.position.z < -200 | transform.position.y < -200)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(.05f);
        GameManager.Instance.Explosion(transform.position, false);
        Destroy(gameObject);
    }
}
