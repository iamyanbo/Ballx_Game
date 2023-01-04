using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public enum ballState {
        aim,
        fire,
        wait
    }
    public ballState currentState;
    private GameObject[] balls = new GameObject[1];
    private Vector2 mouseStartPosition;
    private Vector2 mouseEndPosition;
    private Vector2 mouseCurrentPosition;
    private float ballVelocityX;
    private float ballVelocityY;
    private float VelocityMagnitude = 10f;
    private BrickSpawnController brickSpawnController;
    private bool hasLastBallStopped;
    private int numTurns = 1;
    private NumBallsDisplay numBallsDisplay;
    private int spawnNewBalls = 0;
    private SaveGame saveGame;
    private LoadSavedGame loadSavedGame;
    private GameObject arrow;

    // Start is called before the first frame update
    void Start() {
        brickSpawnController = FindObjectOfType<BrickSpawnController>();
        balls = GameObject.FindGameObjectsWithTag("Ball");
        numBallsDisplay = FindObjectOfType<NumBallsDisplay>();
        saveGame = FindObjectOfType<SaveGame>();
        loadSavedGame = FindObjectOfType<LoadSavedGame>();
        if(loadSavedGame.LoadGameData() != null) {
            numTurns = loadSavedGame.LoadGameData().score;
        }
        currentState = ballState.aim;
        arrow = GameObject.Find("Arrow");
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        switch(currentState) {
            case ballState.aim:
                if(Input.GetMouseButtonDown(0)) {
                    MouseClicked();
                }
                if(Input.GetMouseButton(0)) {
                    MouseDragged();
                }
                if(Input.GetMouseButtonUp(0)) {
                    ReleaseMouse();
                }
                break;
            case ballState.fire:
                break;
            case ballState.wait:
                if(hasLastBallStopped) {
                    numTurns += 1;
                    brickSpawnController.SpawnBricks(numTurns);
                    hasLastBallStopped = false;
                    if(spawnNewBalls > 0) {
                        // Add spawnNewBalls number of balls
                        for(int i = 0; i < spawnNewBalls; i++) {
                            GameObject newBall = Instantiate(balls[0], new Vector3(0, -3.75f, 0), Quaternion.identity);
                            newBall.tag = "Ball";
                            // Ignore collision between balls
                            balls = GameObject.FindGameObjectsWithTag("Ball");
                            foreach(GameObject ball in balls) {
                                Physics2D.IgnoreCollision(ball.GetComponent<Collider2D>(), newBall.GetComponent<Collider2D>());
                            }
                        }
                        spawnNewBalls = 0;
                    }
                    numBallsDisplay.UpdateNumBalls(balls.Length);
                    foreach(GameObject ball in balls) {
                        ball.GetComponent<BallController>().SetCurrentState(BallController.ballState.aim);
                    }
                    currentState = ballState.aim;
                    saveGame.SaveGameData(balls.Length, numTurns);
                }
                break;
            default:
                break;
        }
    }

    public void MouseClicked() {
        mouseStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void MouseDragged() {
        arrow.SetActive(true);
        Vector2 tempMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float diffX = mouseStartPosition.x - tempMousePosition.x;
        float diffY = mouseStartPosition.y - tempMousePosition.y;
        if(diffY <= 0) {
            diffY = 0.05f;
        }
        float angle = Mathf.Rad2Deg * Mathf.Atan(diffX / diffY);
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, -angle);
    }

    public void ReleaseMouse() {
        arrow.SetActive(false);
        mouseEndPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ballVelocityX = (mouseStartPosition.x - mouseEndPosition.x);
        ballVelocityY = (mouseStartPosition.y - mouseEndPosition.y);
        // Check if the velocity is 0, meaning the mouse was not dragged
        if(ballVelocityX == 0 && ballVelocityY == 0) {
            return;
        }
        if(ballVelocityY <= 0) {
            ballVelocityY = 0.1f;
        }
        Vector2 ballVelocity = new Vector2(ballVelocityX, ballVelocityY).normalized;
        currentState = ballState.wait;
        StartCoroutine(FireBall(0, ballVelocity));
    }

    IEnumerator FireBall(int ballIndex, Vector2 ballVelocity) {
        balls[ballIndex].GetComponent<Rigidbody2D>().velocity = ballVelocity * VelocityMagnitude;
        balls[ballIndex].GetComponent<BallController>().SetCurrentState(BallController.ballState.wait);
        yield return new WaitForSeconds(0.2f);
        // Check if there are more balls to fire
        if(ballIndex < balls.Length - 1) {
            StartCoroutine(FireBall(ballIndex + 1, ballVelocity));
        }
    }

    public void SetHasLastBallStopped(bool hasLastBallStopped) {
        if(hasLastBallStopped) {
            this.hasLastBallStopped = true;
            numBallsDisplay.UpdateNumBalls(balls.Length);
        } else {
            this.hasLastBallStopped = false;
        }
    }

    public int GetNumBalls() {
        return balls.Length;
    }

    public int GetNumTurns() {
        return numTurns;
    }

    public void AddNewBall() {
        spawnNewBalls += 1;
    }
}