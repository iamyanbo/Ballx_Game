using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BricksLand : MonoBehaviour {

    private GameObject[] bricks;
    private GameController gameController;
    private BrickSpawnController brickSpawnController;

    // Start is called before the first frame update
    void Start() {
        bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
        // Add the triangle bricks to the bricks array
        GameObject[] triangleBricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
        List<GameObject> brickList = new List<GameObject>(bricks);
        brickList.AddRange(triangleBricks);
        bricks = brickList.ToArray();
        // Add the AddBall bricks to the bricks array
        GameObject[] addBallBricks = GameObject.FindGameObjectsWithTag("AddBall");
        brickList.AddRange(addBallBricks);
        bricks = brickList.ToArray();
        gameController = FindObjectOfType<GameController>();
        brickSpawnController = FindObjectOfType<BrickSpawnController>();
    }

    // Update is called once per frame
    void Update() {
        // Update the bricks array
        bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
        // Add the triangle bricks to the bricks array
        GameObject[] triangleBricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
        List<GameObject> brickList = new List<GameObject>(bricks);
        brickList.AddRange(triangleBricks);
        bricks = brickList.ToArray();
        // Add the AddBall bricks to the bricks array
        GameObject[] addBallBricks = GameObject.FindGameObjectsWithTag("AddBall");
        brickList.AddRange(addBallBricks);
        bricks = brickList.ToArray();
        // Check if any bricks are on the ground
        foreach(GameObject brick in bricks) {
            if(brick.transform.position.y <= -3.5f) {
                if(brick.tag != "AddBall") {
                    // Save the score into a json file
                    SaveIntoJson();
                    // Clear the gameData.json file
                    System.IO.File.WriteAllText(Application.dataPath + "/gameData.json", "");
                    // Switch to game over scene
                    SceneManager.LoadScene("GameOverScene");
                } else {
                    brickSpawnController.RemoveBrick(brick);
                }
            }
        }
    }

    public void SaveIntoJson() {
        gameController = FindObjectOfType<GameController>();
        brickSpawnController = FindObjectOfType<BrickSpawnController>();
        // Check if there is a json file
        if(System.IO.File.Exists(Application.dataPath + "/scoreData.json")) {
            // Read the json file
            string json = System.IO.File.ReadAllText(Application.dataPath + "/scoreData.json");
            ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json);
            // Update the score
            scoreData.score = gameController.GetNumTurns();
            // Update the high score
            if(scoreData.score > scoreData.highScore) {
                scoreData.highScore = scoreData.score;
            }
            // Save the score into a json file
            json = JsonUtility.ToJson(scoreData);
            System.IO.File.WriteAllText(Application.dataPath + "/scoreData.json", json);
        } else {
            // Create a new score data
            ScoreData scoreData = new ScoreData();
            scoreData.score = gameController.GetNumTurns();
            scoreData.highScore = scoreData.score;
            // Save the score into a new json file
            string json = JsonUtility.ToJson(scoreData);
            System.IO.File.WriteAllText(Application.dataPath + "/scoreData.json", json);
        }
    }
}

[System.Serializable]
public class ScoreData {
    public int score;
    public int highScore;
}
