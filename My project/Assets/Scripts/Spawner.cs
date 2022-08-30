using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour
{ private int waveNo=1;
    public GameObject[] enemies;
    public GameObject[] buildings;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(500, -150, 1500), Quaternion.Euler(0, 0, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(-500, -150, 1500), Quaternion.Euler(0, 180, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(700, -150, 1500), Quaternion.Euler(0, 0, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(-700, -150, 1500), Quaternion.Euler(0, 180, 0));
        StartCoroutine("Builder");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Spawn();
            waveNo++;
        }
    }
    void Spawn()
    {
        for (int i = 0; i < waveNo; i++)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(Random.Range(-100, 100), Random.Range(80, 120), 300), Quaternion.Euler(0,-180, 0));
        }
    }
    IEnumerator Builder()
    {
        yield return new WaitForSeconds(0.7f);
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(500, -150,1500), Quaternion.Euler(0, 0, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(-500, -150, 1500), Quaternion.Euler(0, 180, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(700, -150, 1500), Quaternion.Euler(0, 0, 0));
        Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(-700, -150, 1500), Quaternion.Euler(0, 180, 0));
        StartCoroutine("Builder");
    }
}
