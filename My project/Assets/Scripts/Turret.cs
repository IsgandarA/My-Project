using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Turret : MonoBehaviour
{
    [SerializeField] float speed;
    public Player player;
    private bool windUp;
    private bool windUp2stage;
    public AudioClip windUpSound;
    public AudioClip shootSound;
    public AudioSource audio;
    [SerializeField] GameObject turret;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject barrels;
    [SerializeField] GameObject bulletSpawn;
    [SerializeField] Camera cam;
    public Vector3 look;
    private bool cooldown;
    public Vector3 mousePos;
    private LineRenderer lr;
    [SerializeField] ParticleSystem muzzleFlash;
    //private Vector3 mousePos;
    //private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        lr = GetComponent<LineRenderer>();
        //mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.z*-1));
        audio = GetComponent<AudioSource>();
        audio.loop = true;
        audio.clip = shootSound;
        audio.Stop();
        audio.volume = 0.1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine("WindUp");
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            windUp = false;
            audio.Stop();
            StopCoroutine("WindUp");
            windUp2stage = false;
        }
        if (windUp&&!windUp2stage)
        {
            barrels.transform.Rotate(0.0f, 0, speed*4, Space.Self);
            if (!cooldown)
            {
                StartCoroutine("Shoot");
            }
            
        }
        if (windUp2stage)
        {
            barrels.transform.Rotate(0.0f, 0, speed*2, Space.Self);
        }
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            look = hit.transform.position;
        }
        else
        {
            look = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        }
        Vector3 lookTurret = Vector3.RotateTowards(transform.position, look, 180, 180);
        transform.LookAt(lookTurret);
        lr.positionCount = 2;
        List<Vector3> points = new List<Vector3>();
        Vector3 startPos = transform.position;
        Vector3 endPos = look;
        points.Add(startPos);
        points.Add(endPos);
        lr.SetPositions(points.ToArray());
        bullet.transform.LookAt(look);
    }

    IEnumerator WindUp()
    {
        windUp2stage = true;
        audio.PlayOneShot(windUpSound, 1f);
        yield return new WaitForSeconds(.6f);
        windUp2stage = false;
        windUp = true;
        audio.Play();
    }
    IEnumerator Shoot()
    {
        cooldown = true;
        Instantiate(bullet, bulletSpawn.transform.position, Quaternion.Euler(bulletSpawn.transform.eulerAngles.x, bulletSpawn.transform.eulerAngles.y, bulletSpawn.transform.eulerAngles.z));
        muzzleFlash.Play();
        yield return new WaitForSeconds(.3f);
        cooldown = false;

    }
}
