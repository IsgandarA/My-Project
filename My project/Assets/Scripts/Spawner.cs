using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    private int waveNo=1;
    [SerializeField] GameObject boss;
    public GameObject[] enemies;
    public GameObject[] buildings;
    public GameObject[] buildingsChal;

    private bool waveOver = true;
   
    private bool moveChall= false;
    private bool bossTime;
    public int level;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        level = 1;
        StartCoroutine(Builder());
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {


    }
     IEnumerator Spawn()
    {
        
        if (level <= 4)
        
        for (int i = 0; i < waveNo; i++)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(Random.Range(-100, 100), Random.Range(150, 350), 500), Quaternion.Euler(0,-180, 0));
        } 
        yield return new WaitUntil(()=> GameObject.FindGameObjectsWithTag("Enemy").Length == 0&& GameObject.FindGameObjectsWithTag("Boss").Length == 0);
        yield return new WaitForSeconds(2);
        for (int i = 0; i < waveNo; i++)
        {
            if (Player.Instance.transform.position.x < 250 && Player.Instance.transform.position.x > -250&& Player.Instance.transform.position.x!=0)
            {
            Instantiate(buildingsChal[Random.Range(0, buildingsChal.Length)], new Vector3(Player.Instance.transform.position.x, 0, 900), Quaternion.Euler(0, 0, 0));
            Instantiate(buildingsChal[Random.Range(0, buildingsChal.Length)], new Vector3(Random.Range(100, 150) * (Player.Instance.transform.position.x / Mathf.Abs(Player.Instance.transform.position.x)), 0, 1200), Quaternion.Euler(0, 0, 0));
            Instantiate(buildingsChal[Random.Range(0, buildingsChal.Length)], new Vector3(Random.Range(350, 450) * (Player.Instance.transform.position.x / Mathf.Abs(Player.Instance.transform.position.x)), 0, 1200), Quaternion.Euler(0, 0, 0));
            }

            //else if (Player.Instance.transform.position.x < 250 && Player.Instance.transform.position.x > -250 && level > 2)
            //{
            //    Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(Player.Instance.transform.position.x, -50, 1200 * (level / 3f)), Quaternion.Euler(0, 0, 0));
            //    Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(Player.Instance.transform.position.x + 280 * (Player.Instance.transform.position.x / Mathf.Abs(Player.Instance.transform.position.x) * -1), -50, 1200 * (level / 3f)), Quaternion.Euler(0, 0, 0));
            //}
            else
        {
                Instantiate(buildingsChal[Random.Range(0, buildingsChal.Length)], new Vector3(200, 0, 1200), Quaternion.Euler(0, 0, 0));
                Instantiate(buildingsChal[Random.Range(0, buildingsChal.Length)], new Vector3(-200, 0, 1200), Quaternion.Euler(0, 0, 0));

                Instantiate(buildingsChal[Random.Range(0, buildingsChal.Length)], new Vector3(Player.Instance.transform.position.x, -50, 900), Quaternion.Euler(0, 0, 0));
        }
            yield return new WaitForSeconds(1f);

        }
        
        
        yield return new WaitForSeconds(2);
        waveNo++;
        //if (level <= 4 && waveNo % 5 == 0)
        //{
        //    waveNo = 1;
        //    level += 1;
        //    Player.Instance.Health = 30;
        //}
        level = waveNo;
        if (level < 4)
        {
        StartCoroutine(Spawn());
        }
        else
        {
            Instantiate(boss, new Vector3(0, 250, 300), Quaternion.Euler(0, -180, 0));
        }
    }
    IEnumerator Builder()
    {
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(Random.Range(500, 600), -0,2000), Quaternion.Euler(0, 0, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(Random.Range(-500, -600), -0, 2000), Quaternion.Euler(0, 180, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(Random.Range(-700, -850), -0, 1700), Quaternion.Euler(0, 0, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(Random.Range(700, 850), -0, 1700), Quaternion.Euler(0, 180, 0));
        if (level < 4)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        StartCoroutine("Builder");
    }
    
}
