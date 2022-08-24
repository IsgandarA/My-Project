using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour
{ private int waveNo=1;
    public GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        
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
            Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(Random.Range(-30, 30), Random.Range(12, 24), 100), Quaternion.Euler(0,-180, 0));
        }
    }
}
