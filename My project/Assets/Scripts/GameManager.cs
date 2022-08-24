using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] AudioClip explSound;
    [SerializeField] AudioClip muflExpl;
    [SerializeField] GameObject explosion;
    
    private AudioSource audio;
    public static GameManager Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Explosion(Vector3 pos, bool hit)
    {
        audio.transform.position = pos;
        if (hit)
        {
            audio.PlayOneShot(explSound, 0.3f);
            explosion.transform.position = pos;
            Instantiate(explosion, pos, explosion.transform.rotation);
        }
        else
        {
            audio.PlayOneShot(muflExpl, 0.3f);
        }
        explosion.transform.position = pos;

    }
}
