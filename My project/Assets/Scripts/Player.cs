using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Helicopter
{
    public static Player Instance;
    [SerializeField]float speed;
    private int x;
    [SerializeField] GameObject[] rocketsSlots;
    [SerializeField] GameObject[] rockets;
    [SerializeField] GameObject rocket;
    [SerializeField] Turret turret;
    public AudioSource audio;
    public bool lockedRocketBool;
    public AudioClip lockOn;
    private float horInput;
    private float verInput;
    private Rigidbody playerRB;
    public bool dodgeCooldown = false;
    private static List<int> negPos = new List<int>() { -1, 1};
    public bool shooting;
    public Vector3 vel;

    [SerializeField] GameObject sparks;
    Vector3 lastPos;
    float bounds = 200;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        turret = GameObject.Find("Turret").GetComponent<Turret>();
        x = 0;
        health = 20;
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    
}
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Building"))
        {
            Health -= 3;
            Instantiate(sparks, transform.position, sparks.transform.rotation);
        }
 
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        speed = 2000;
        playerRB = GetComponent<Rigidbody>();
        Rotator(true);
        playerRB.maxAngularVelocity = 100000;
    }
    void Attack(int x)
    {
        if (turret.lockedOn)
        {
            audio.PlayOneShot(lockOn);
            StartCoroutine("lockedRocket");
        }
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
            }
            if (x == 3)
            {
                x = 0;
            }
        }

    }
    // Update is called once per frame
     void FixedUpdate()
    {
      
        vel = playerRB.velocity;
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");
        if (horInput != 0 && !dodgeCooldown || verInput != 0 && !dodgeCooldown)
        {
            Move();
        }
        if (Input.GetKey(KeyCode.Space)&&!dodgeCooldown)
        {
            StartCoroutine(Dodge());
        }
        if (health < 0.1)
        {
            GameOver();
        }
        Bounds(bounds);
        if (!Input.anyKey && !dodgeCooldown)
        {
            playerRB.velocity = playerRB.velocity * 0.95f;
        }
        if (transform.position.x < -400)
        {
            playerRB.AddForce(Vector3.right * 1500f, ForceMode.Impulse);
        }
        if (transform.position.x > 400)
        {
            playerRB.AddForce(Vector3.left * 1500f, ForceMode.Impulse);
        }
    }
    IEnumerator lockedRocket()
    {
        lockedRocketBool = true;
        yield return new WaitForSeconds(1);
        lockedRocketBool = false;

    }
    void Bounds(float b)
    {
        //if (transform.position.x > b || transform.position.x < -b)
        //{
        //    transform.position = new Vector3(transform.position.x*-1, transform.position.y, transform.position.z);
        //    playerRB.AddForce(new Vector3(transform.position.x*-1, 0, 0), ForceMode.Impulse);
        //}
        //if (transform.position.y > 250)
        //{

        //}

        //if (transform.position.y > b* 2f || transform.position.y < 0)
        //{
        //    transform.position = new Vector3(transform.position.x, b*2f - (transform.position.y), transform.position.z);
        //    playerRB.AddForce(new Vector3(0, b* 2f - transform.position.y, 0), ForceMode.Impulse);
        //}
    }
    public void GameOver()
    {
        SceneManager.LoadScene(0);
        if (Title.Instance.score > Title.Instance.highScoreInt)
        {
            Title.Instance.highScoreInt = Title.Instance.score;
            Title.Instance.playerScoreHist[Title.Instance.playerN] = Title.Instance.highScoreInt;
            Title.Instance.SaveScore();
        }

        Destroy(gameObject);
        SceneManager.LoadScene(0);
        Title.Instance.Restart();
        Title.Instance.playerHealth.gameObject.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && !dodgeCooldown)
        {
            Attack(x);
        }
        if (Title.Instance.playerHealth != null)
        {
            Title.Instance.playerHealth.value = health;

        }

    }
    protected override void Move()
    {
        playerRB.velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.D)&&playerRB.velocity.x <200 && transform.position.x < 390)
            {
                playerRB.AddForce(new Vector3(speed, 0, 0), ForceMode.Impulse);

            }
            if (Input.GetKey(KeyCode.A) && playerRB.velocity.x > -200 && transform.position.x > -390)
            {
            playerRB.AddForce(new Vector3(-speed, 0, 0), ForceMode.Impulse);

            }
            if (Input.GetKey(KeyCode.W) && playerRB.velocity.y < 200 && transform.position.y <500)
            {

                playerRB.AddForce(new Vector3(0, speed*2, 0), ForceMode.Impulse);

            }
            if (Input.GetKey(KeyCode.S) && playerRB.velocity.x > -200 && transform.position.y > 0)
            {
                playerRB.AddForce(new Vector3(0, -speed*2, 0), ForceMode.Impulse);
         
            }


    }


    void Rotator(bool x)
    {if (x)
        {
            playerRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
        }
     if (!x)
        {
            playerRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
        }
    }
    IEnumerator Dodge()
        {
        Rotator(false);
        dodgeCooldown = true;
        playerRB.velocity = Vector3.zero;
        if (horInput > 0)
        {
            playerRB.AddForce(new Vector3(1, 0, 0)* 2300f, ForceMode.Impulse);
            playerRB.AddRelativeTorque(new Vector3(0, 0, 8.5f), ForceMode.Impulse);

        }
        else if (horInput < 0)
        {
            playerRB.AddForce(Vector3.left * 2300f, ForceMode.Impulse);
            playerRB.AddRelativeTorque(new Vector3(0, 0, 8.5f), ForceMode.Impulse);

        }
        else if (horInput == 0)
        {
            playerRB.AddRelativeTorque(new Vector3(0, 0, 8.5f)*negPos[Random.Range(0, 2)], ForceMode.Impulse);
        }

        yield return new WaitForSeconds(.7f);
        playerRB.angularVelocity = Vector3.zero;
        playerRB.velocity = playerRB.velocity*.85f;
        playerRB.MoveRotation(Quaternion.Euler(0, 0, 0));
        dodgeCooldown = false;
        Rotator(true);
    }

    IEnumerator RocketRespawn(GameObject slot)
    {
        yield return new WaitForSeconds(2);
        rockets[System.Array.IndexOf(rocketsSlots, slot)] =Instantiate(rocket, slot.transform.position, slot.transform.rotation);
        rockets[System.Array.IndexOf(rocketsSlots, slot)].tag = "PlayerRocket";
        rockets[System.Array.IndexOf(rocketsSlots, slot)].transform.parent = slot.transform;
    }
}
