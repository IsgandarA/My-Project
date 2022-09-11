using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
public class Title : MonoBehaviour
{
    public static Title Instance;
    public Dictionary<string, int> playerScoreHist = new Dictionary<string, int>();
    public int highScoreInt;
    public int score;
    public Button start;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI victoryText;

    public TextMeshProUGUI highScore;
    public TextMeshProUGUI user;
    public Slider playerHealth;
    public Slider bossHealth;

    public string playerN;
    private bool nameInped;
    private void Awake()
    {
        LoadScore();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
   
        Restart();
    }
    // Start is called before the first frame update
    public void Restart()
    {

        playerName.gameObject.SetActive(true);
        score = 0;
        start.gameObject.SetActive(false);
        user.gameObject.SetActive(false);
        playerName.text = "Player: ";
        nameInped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!nameInped)
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b')
                {
                    if (playerName.text.Length != 8)
                    {
                        playerName.text = playerName.text.Substring(0, playerName.text.Length - 1);

                    }

                }
                else if ((c == '\n') || (c == '\r'))
                {
                    nameInped = true;
                    playerN = playerName.text.Substring(9, playerName.text.Length - 9);
                    if (playerScoreHist.ContainsKey(playerN))
                    {
                        highScoreInt = playerScoreHist[playerN];
                    }
                    else
                    {
                        playerScoreHist.Add(playerN, 0);
                    }
                    highScore.text = "High score: " + playerScoreHist[playerN].ToString();
                    Next();
                }
                else
                {
                    playerName.text = playerName.text + c;
                }
            }
        }

    }
    public void Next()
    {
        user.text = playerName.text;
        playerName.gameObject.SetActive(false);
        start.gameObject.SetActive(true);
        user.gameObject.SetActive(true);
    }
    public void StartGame()
    {

        SceneManager.LoadScene(1);
        start.gameObject.SetActive(false);
        highScore.text = "Score: ";
        playerHealth.gameObject.SetActive(true);
    }
    [Serializable]
    class SaveData
    {
        public List<string> playersList;
        public List<int> playerScores;
    }
    public void SaveScore()
    {
        SaveData updPlayers = new SaveData();
        updPlayers.playersList = new List<String>(playerScoreHist.Keys);
        updPlayers.playerScores = new List<int>(playerScoreHist.Values);
        string json = JsonUtility.ToJson(updPlayers);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData updPlayers = JsonUtility.FromJson<SaveData>(json);
            foreach (string player in updPlayers.playersList)
            {
                playerScoreHist.Add(player, updPlayers.playerScores[updPlayers.playersList.IndexOf(player)]);
            }

        }
    }
    public IEnumerator Victory()
    {
        victoryText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        victoryText.gameObject.SetActive(false);
        Player.Instance.GameOver();
    }
}
