using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Helicopter
{
    
    [SerializeField]float speed;
    private int x;
    [SerializeField] GameObject[] rocketsSlots;
    [SerializeField] GameObject[] rockets;
    [SerializeField] GameObject rocket;
    private float horInput;
    private float verInput;
    private Rigidbody playerRB;
    private bool dodgeCooldown = false;
    private static List<int> negPos = new List<int>() { -1, 1};
    public bool shooting;
    public Vector3 vel;
    public static Player Instance;
    Vector3 lastPos;
    private void Awake()
    {
        x = 0;
        health = 10;
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    
}
    // Start is called before the first frame update
    protected override void Start()
    {
        vertical = 200;
        horizontal = 200;
        bounds[0] = vertical;
        bounds[1] = horizontal;
        boundsBounce = 20;
        speed = 60;
        playerRB = GetComponent<Rigidbody>();
        Rotator(true);
        playerRB.maxAngularVelocity = 100000;
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
            }
            if (x == 3)
            {
                x = 0;
            }
        }

    }
    // Update is called once per frame
    protected override void FixedUpdate()
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
            Destroy(gameObject);
            
            Debug.Log("Game over");
        }
        Bounds(bounds);



    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && !dodgeCooldown)
        {
            Attack(x);
        }
        if (!Input.anyKey && !dodgeCooldown)
        {
            playerRB.velocity = playerRB.velocity*0.995f;
        }

    }
    protected override void Move()
    {
            if (Input.GetKey(KeyCode.D))
            {
                playerRB.AddForce(new Vector3(speed, 0, 0), ForceMode.Acceleration);

            }
            if (Input.GetKey(KeyCode.A))
            {
                playerRB.AddForce(new Vector3(-speed, 0, 0), ForceMode.Acceleration);

            }
            if (Input.GetKey(KeyCode.W))
            {
                playerRB.AddForce(new Vector3(0, speed, 0), ForceMode.Acceleration);

            }
            if (Input.GetKey(KeyCode.S))
            {
                playerRB.AddForce(new Vector3(0, -speed, 0), ForceMode.Acceleration);
         
            }
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            health--;
            Destroy(collision.gameObject);
            playerRB.AddForce(new Vector3(0, -1, 0)*2000f, ForceMode.Impulse);
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
            playerRB.AddForce(new Vector3(1, 0, 0)* 700f, ForceMode.Impulse);
            playerRB.AddRelativeTorque(new Vector3(0, 0, -180), ForceMode.Impulse);
            
        }
        else if (horInput < 0)
        {
            playerRB.AddForce(Vector3.left * 700f, ForceMode.Impulse);
            playerRB.AddRelativeTorque(new Vector3(0, 0,180), ForceMode.Impulse);
        }
        else if (horInput == 0)
        {
            playerRB.AddRelativeTorque(new Vector3(0, 0, 180)*negPos[Random.Range(0, 2)], ForceMode.Impulse);
        }

        yield return new WaitForSeconds(.7f);
        playerRB.angularVelocity = Vector3.zero;
        playerRB.velocity = playerRB.velocity*.9f;
        playerRB.MoveRotation(Quaternion.Euler(0, 0, 0));
        dodgeCooldown = false;
        Rotator(true);


    }
    IEnumerator RocketRespawn(GameObject slot)
    {
        yield return new WaitForSeconds(2);
        rockets[System.Array.IndexOf(rocketsSlots, slot)] = (GameObject)GameObject.Instantiate(rocket, slot.transform.position, slot.transform.rotation);
        rockets[System.Array.IndexOf(rocketsSlots, slot)].transform.parent = slot.transform;
    }
}
