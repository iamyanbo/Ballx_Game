using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveGame : MonoBehaviour {

    private BricksLand bricksLand;
    private GameController gameController;

    // Start is called before the first frame update
    void Start() {
        bricksLand = FindObjectOfType<BricksLand>();
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SaveGameData() {
        bricksLand = FindObjectOfType<BricksLand>();
        GameData gameData = new GameData();
        // Save bricks
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
        gameData.squareBrickX = new float[bricks.Length];
        gameData.squareBrickY = new float[bricks.Length];
        gameData.squareBrickRotation = new int[bricks.Length];
        gameData.squareBrickHealth = new int[bricks.Length];
        for(int i = 0; i < bricks.Length; i++) {
            gameData.squareBrickX[i] = bricks[i].transform.position.x;
            gameData.squareBrickY[i] = bricks[i].transform.position.y;
            gameData.squareBrickRotation[i] = (int)bricks[i].transform.rotation.eulerAngles.z;
            gameData.squareBrickHealth[i] = int.Parse(bricks[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text);
        }
        bricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
        gameData.TriangleBrickX = new float[bricks.Length];
        gameData.TriangleBrickY = new float[bricks.Length];
        gameData.TriangleBrickRotation = new int[bricks.Length];
        gameData.TriangleBrickHealth = new int[bricks.Length];
        for(int i = 0; i < bricks.Length; i++) {
            gameData.TriangleBrickX[i] = bricks[i].transform.position.x;
            gameData.TriangleBrickY[i] = bricks[i].transform.position.y;
            gameData.TriangleBrickRotation[i] = (int)bricks[i].transform.rotation.eulerAngles.z;
            gameData.TriangleBrickHealth[i] = int.Parse(bricks[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text);
        }
        bricks = GameObject.FindGameObjectsWithTag("AddBall");
        gameData.addBallX = new float[bricks.Length];
        gameData.addBallY = new float[bricks.Length];
        for(int i = 0; i < bricks.Length; i++) {
            gameData.addBallX[i] = bricks[i].transform.position.x;
            gameData.addBallY[i] = bricks[i].transform.position.y;
        }
        // Get gameData json file
        string json = System.IO.File.ReadAllText(Application.dataPath + "/gameData.json");
        GameData gameData1 = JsonUtility.FromJson<GameData>(json);
        // Check if gameData json file is empty
        if(json == "") {
            gameData1 = new GameData();
        }
        // Save balls
        gameData.numBalls = gameData1.numBalls;
        // Save score
        gameData.score = gameData1.score;
        // Save gameData to json file
        string json1 = JsonUtility.ToJson(gameData);
        System.IO.File.WriteAllText(Application.dataPath + "/gameData.json", json1);
        bricksLand.SaveIntoJson();
        // Update scoreData json file
        string json2 = System.IO.File.ReadAllText(Application.dataPath + "/scoreData.json");
        ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json2);
        if(scoreData.highScore < gameData.score) {
            scoreData.highScore = gameData.score;
        }
        scoreData.score = gameData.score;
        string json3 = JsonUtility.ToJson(scoreData);
        System.IO.File.WriteAllText(Application.dataPath + "/scoreData.json", json3);
    }

    public void SaveGameData(int numBalls, int score) {
        bricksLand = FindObjectOfType<BricksLand>();
        GameData gameData = new GameData();
        // Save bricks
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
        gameData.squareBrickX = new float[bricks.Length];
        gameData.squareBrickY = new float[bricks.Length];
        gameData.squareBrickRotation = new int[bricks.Length];
        gameData.squareBrickHealth = new int[bricks.Length];
        for(int i = 0; i < bricks.Length; i++) {
            gameData.squareBrickX[i] = bricks[i].transform.position.x;
            gameData.squareBrickY[i] = bricks[i].transform.position.y;
            gameData.squareBrickRotation[i] = (int)bricks[i].transform.rotation.eulerAngles.z;
            gameData.squareBrickHealth[i] = int.Parse(bricks[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text);
        }
        bricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
        gameData.TriangleBrickX = new float[bricks.Length];
        gameData.TriangleBrickY = new float[bricks.Length];
        gameData.TriangleBrickRotation = new int[bricks.Length];
        gameData.TriangleBrickHealth = new int[bricks.Length];
        for(int i = 0; i < bricks.Length; i++) {
            gameData.TriangleBrickX[i] = bricks[i].transform.position.x;
            gameData.TriangleBrickY[i] = bricks[i].transform.position.y;
            gameData.TriangleBrickRotation[i] = (int)bricks[i].transform.rotation.eulerAngles.z;
            gameData.TriangleBrickHealth[i] = int.Parse(bricks[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text);
        }
        bricks = GameObject.FindGameObjectsWithTag("AddBall");
        gameData.addBallX = new float[bricks.Length];
        gameData.addBallY = new float[bricks.Length];
        for(int i = 0; i < bricks.Length; i++) {
            gameData.addBallX[i] = bricks[i].transform.position.x;
            gameData.addBallY[i] = bricks[i].transform.position.y;
        }
        // Save balls
        gameData.numBalls = numBalls;
        // Save score
        gameData.score = score;
        // Save gameData to json file
        string json1 = JsonUtility.ToJson(gameData);
        System.IO.File.WriteAllText(Application.dataPath + "/gameData.json", json1);
        bricksLand.SaveIntoJson();
        // Update scoreData json file
        string json2 = System.IO.File.ReadAllText(Application.dataPath + "/scoreData.json");
        ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json2);
        if(scoreData.highScore < gameData.score) {
            scoreData.highScore = gameData.score;
        }
        scoreData.score = gameData.score;
        string json3 = JsonUtility.ToJson(scoreData);
        System.IO.File.WriteAllText(Application.dataPath + "/scoreData.json", json3);
    }
}

[System.Serializable]
public class GameData {
    public float[] squareBrickX;
    public float[] squareBrickY;
    public int[] squareBrickRotation;
    public int[] squareBrickHealth;
    public float[] TriangleBrickX;
    public float[] TriangleBrickY;
    public int[] TriangleBrickRotation;
    public int[] TriangleBrickHealth;
    public float[] addBallX;
    public float[] addBallY;
    public int numBalls = 1;
    public int score = 1;
    public bool returnToResumedGame = false;

    public void SetReturnToResumedGame(bool returnToResumedGame) {
        this.returnToResumedGame = returnToResumedGame;
    }

    public bool GetReturnToResumedGame() {
        return returnToResumedGame;
    }
}