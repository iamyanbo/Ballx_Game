using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour {

    private GameObject scoreBox;

    // Start is called before the first frame update
    void Start() {
        scoreBox = GameObject.Find("Score");
        UpdateScore();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void UpdateScore() {
        // Read from scoreData json file
        string json = System.IO.File.ReadAllText(Application.dataPath + "/scoreData.json");
        ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json);
        scoreBox.GetComponentInChildren<TMP_Text>().text = "Score: " + scoreData.score.ToString() + "\nHigh Score: " + scoreData.highScore.ToString();
    }
}
