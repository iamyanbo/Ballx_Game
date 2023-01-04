using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadSavedGame : MonoBehaviour {

    private GameObject squareBrick;
    private GameObject triangleBrick;
    private GameObject ball;
    private GameObject addball;
    private GameObject canvas;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void LoadGame() {
        squareBrick = GameObject.Find("SquareBrick");
        triangleBrick = GameObject.Find("TriangleBrick");
        ball = GameObject.Find("Ball");
        addball = GameObject.Find("AddBall");
        canvas = GameObject.Find("NumberBallsCanvas");
        // Clear all the bricks from the scene
        GameObject[] bricksToDestroy = GameObject.FindGameObjectsWithTag("SquareBrick");
        for(int i = 0; i < bricksToDestroy.Length; i++) {
            Destroy(bricksToDestroy[i]);
        }
        bricksToDestroy = GameObject.FindGameObjectsWithTag("TriangleBrick");
        for(int i = 0; i < bricksToDestroy.Length; i++) {
            Destroy(bricksToDestroy[i]);
        }
        // Load the saved game data
        GameData gameData = LoadGameData();
        if(gameData == null) {
            Debug.Log("No game data to load");
            return;
        }
        if(gameData.GetReturnToResumedGame()) {
            // Populate the scene with the saved game data
            for(int i = 0; i < gameData.squareBrickX.Length; i++) {
                GameObject squareBrick = Instantiate(this.squareBrick);
                squareBrick.transform.position = new Vector3(gameData.squareBrickX[i], gameData.squareBrickY[i], gameData.squareBrickRotation[i]);
                squareBrick.tag = "SquareBrick";
            }
            for(int i = 0; i < gameData.TriangleBrickX.Length; i++) {
                GameObject triangleBrick = Instantiate(this.triangleBrick);
                triangleBrick.transform.position = new Vector3(gameData.TriangleBrickX[i], gameData.TriangleBrickY[i], 0);
                triangleBrick.tag = "TriangleBrick";
            }
            // Rotate the bricks to the saved rotation
            GameObject[] bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
            if(bricks.Length != 0) {
                for(int i = 0; i < bricks.Length; i++) {
                    bricks[i].transform.rotation = Quaternion.Euler(0, 0, gameData.squareBrickRotation[i]);
                }
            }
            bricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
            if(bricks.Length != 0) {
                for(int i = 0; i < bricks.Length; i++) {
                    bricks[i].transform.rotation = Quaternion.Euler(0, 0, gameData.TriangleBrickRotation[i]);
                }
            }
            // Rotate the brick's health text back and set the health
            bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
            if(bricks.Length != 0) {
                Debug.Log(bricks.Length);
                for(int i = 0; i < bricks.Length; i++) {
                    bricks[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = gameData.squareBrickHealth[i].ToString();
                    bricks[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            bricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
            if(bricks.Length != 0) {
                for(int i = 0; i < bricks.Length; i++) {
                    bricks[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = gameData.TriangleBrickHealth[i].ToString();
                    bricks[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            // Add the addball powerups
            for(int i = 0; i < gameData.addBallX.Length; i++) {
                GameObject addball = Instantiate(this.addball);
                addball.transform.position = new Vector3(gameData.addBallX[i], gameData.addBallY[i], 0);
                addball.tag = "AddBall";
            }
            // Add the balls
            for(int i = 0; i < gameData.numBalls - 1; i++) {
                GameObject ball = Instantiate(this.ball);
                ball.transform.position = new Vector3(0, -3.75f, 0);
            }
            // Ignore collisions between balls
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            for(int i = 0; i < balls.Length; i++) {
                for(int j = 0; j < balls.Length; j++) {
                    if(i != j) {
                        Physics2D.IgnoreCollision(balls[i].GetComponent<Collider2D>(), balls[j].GetComponent<Collider2D>());
                    }
                }
            }
            // Update the number of balls display
            canvas.GetComponentInChildren<TMP_Text>().text = gameData.numBalls.ToString();
            gameData.SetReturnToResumedGame(false);
            string json = JsonUtility.ToJson(gameData);
            System.IO.File.WriteAllText(Application.dataPath + "/gameData.json", json);
        }
    }

    public GameData LoadGameData() {
        string json = System.IO.File.ReadAllText(Application.dataPath + "/gameData.json");
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        return gameData;
    }
}
