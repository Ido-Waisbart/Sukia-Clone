using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using Newtonsoft.Json;
using TMPro;

public class SuikaGameManager : MonoBehaviour
{
    public static SuikaGameManager Instance;
    [SerializeField] public ScoreManager scoreManager;
    [SerializeField] protected FruitShooter fruitShooter;

    [SerializeField] protected int totalGameTimeInSeconds = 30;  // No need for the whole 5 minutes.
    [SerializeField] protected GameObject fruitPrefab;  // ASSUMPTION: This has Fruit.cs.
    [SerializeField] protected Transform fruitGroup;
    
    [SerializeField] protected TMP_Text gameTimerText;
    [SerializeField] protected TMP_Text gameOverText;
    [SerializeField] protected SceneReference leaderboardScene;
    
    private float gameStartTime;
    public bool IsGameOngoing{
        get;
        private set;} = true;
    
    void Start()
    {
        if(Instance != null){
            throw new Exception("Singleton.");
        }
        Instance = this;

        // TODO OPTIONAL: Have a countdown, before starting.
        gameStartTime = Time.time;
    }

    void Update(){
        if(IsGameOngoing){
            UpdateGameTimer();
            if(Time.time > gameStartTime + totalGameTimeInSeconds){
                GameOver("Time\nout!");
            }
        }
    }

    void UpdateGameTimer(){
        int timeRemainderInSeconds = (int)(totalGameTimeInSeconds - Mathf.Floor(Time.time - gameStartTime));
        // elapsed = Time.time - gameStartTime
        // elapsed = 0 -> 5:00 (300)
        // elapsed = 0.5 -> 5:00 (300)
        // elapsed = 30 -> 4:30 (270)
        // elapsed = 299.5 -> 0:01 (1)
        // elapsed = 300 -> 0:00 (0)
        int timeRemainder_seconds = timeRemainderInSeconds % 60;
        int timeRemainder_minutes = timeRemainderInSeconds / 60;
        
        gameTimerText.text = "" + timeRemainder_minutes.ToString("00") + ":" + timeRemainder_seconds.ToString("00");
    }

    public void MergeFruit(Fruit fruit, Fruit otherFruit){
        FruitData newFruitData = fruit.FruitData.NextFruit;

        Vector3 midpointPosition = (fruit.transform.position + otherFruit.transform.position) / 2;
        scoreManager.AddToScore(fruit.FruitData.BasePoints, midpointPosition);

        if(newFruitData == null){
            // Likely the last fruit, the sun. Just let it pop.
            return;
        }

        GameObject newFruitGO = Instantiate(fruitPrefab, midpointPosition, Quaternion.identity, fruitGroup);
        Fruit newFruit = newFruitGO.GetComponent<Fruit>();
        if(newFruit == null)
            throw new Exception("Shot fruit must have Fruit.cs.");
        newFruit.Initialize(newFruitData);
    }

    void GameOver(string gameOverString){
        StartCoroutine(IGameOver(gameOverString));
    }

    // Extraneous JSON logic. Irrelevant to this test.
    /*// Put in a new file.
    public class GameHistoryJsonFormat{
        // public GameHistoryRecordJsonFormat[] Records;
        public List<GameHistoryRecordJsonFormat> Records;
    }
    public class GameHistoryRecordJsonFormat{
        public string GameName = "Taichu Kash";
        public string ProgressStatus;
        // public int EarnedCoins;
        public int CompetitionPlacement;
        public DateTime playDate;
        public float EntryFeeInGems;
    }*/

    IEnumerator IGameOver(string gameOverString){
        IsGameOngoing = false;
        // print("DONE!");
        gameOverText.gameObject.SetActive(true);
        // gameOverText.GetComponent<Animator>().Play("Appear");  // TODO OPTIONAL: Animator with fade in
        gameOverText.text = gameOverString;
        
        // Extraneous JSON logic. Irrelevant to this test.
        /*string historyJsonString = PlayerPrefs.GetString("historyJson", "{}");
        print("Before: " + historyJsonString);
        GameHistoryJsonFormat historyJson = JsonConvert.DeserializeObject<GameHistoryJsonFormat>(historyJsonString);
        if(historyJson.Records == null) historyJson.Records = new List<GameHistoryRecordJsonFormat>();
        historyJson.Records.Add(new GameHistoryRecordJsonFormat(){
            ProgressStatus = "Over",
            CompetitionPlacement = 1,  // Dummy value
            playDate = DateTime.Now,  // Dummy value
            EntryFeeInGems = 123,  // Dummy value
        });

        string newHistoryJsonString = JsonConvert.SerializeObject(historyJson);
        //print(JsonUtility.ToJson(historyJson));  // JsonUtility.ToJson() doesn't work with Lists.
        PlayerPrefs.SetString("historyJson", newHistoryJsonString);
        print("After: " + newHistoryJsonString);*/
        
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(leaderboardScene.BuildIndex);
    }

    public void FruitCrossedEndLine(){
        GameOver("Line\ncrossed!");
    }
}
