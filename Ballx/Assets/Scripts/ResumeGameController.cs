using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeGameController : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ClickResume() {
        string json = System.IO.File.ReadAllText(Application.dataPath + "/gameData.json");
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        if(gameData == null) {
            SceneManager.LoadScene("GameScene");
        } else {
            gameData.SetReturnToResumedGame(true);
            json = JsonUtility.ToJson(gameData);
            System.IO.File.WriteAllText(Application.dataPath + "/gameData.json", json);
            SceneManager.LoadScene("GameScene");
        }
    }
}