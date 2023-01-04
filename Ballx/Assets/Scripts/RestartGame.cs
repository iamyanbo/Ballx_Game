using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ClickStart() {
        // Clear the game data file
        GameData gameData = new GameData();
        string json = JsonUtility.ToJson(gameData);
        System.IO.File.WriteAllText(Application.dataPath + "/gameData.json", json);
        SceneManager.LoadScene("gameScene");
    }
}
