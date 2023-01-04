using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStop : MonoBehaviour {

    private GameObject[] balls;
    private GameController gameController;

    // Start is called before the first frame update
    void Start() {
        // Get all balls
        balls = GameObject.FindGameObjectsWithTag("Ball");
        // Get ball controller
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Ball") {
            // Stop the specific ball
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            // Return the ball to origin
            collision.gameObject.transform.position = new Vector3(0, -3.75f, 0);
            // Check if all balls have stopped
            bool allBallsStopped = true;
            balls = GameObject.FindGameObjectsWithTag("Ball");
            foreach(GameObject ball in balls) {
                if(ball.GetComponent<Rigidbody2D>().velocity != Vector2.zero) {
                    allBallsStopped = false;
                    break;
                }
            }
            gameController.SetHasLastBallStopped(allBallsStopped);
        }
    }
}
