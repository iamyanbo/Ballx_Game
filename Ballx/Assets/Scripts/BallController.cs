using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour {

    public enum ballState {
        aim,
        fire,
        wait
    }
    public ballState currentState;
    private BrickSpawnController brickSpawnController;

    // Start is called before the first frame update
    void Start() {
        currentState = ballState.aim;
        brickSpawnController = FindObjectOfType<BrickSpawnController>();
    }

    // Update is called once per frame
    void Update() {
        switch(currentState) {
            case ballState.aim:
                break;
            case ballState.fire:
                break;
            case ballState.wait:
                CheckForOutOfBounds();
                break;
            default:
                break;
        }
    }

    public string GetCurrentState() {
        return currentState.ToString();
    }

    public void SetCurrentState(ballState state) {
        currentState = state;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "SquareBrick" || collision.gameObject.tag == "TriangleBrick") {
            // get the brick object
            var brick = collision.gameObject;
            Component[] components = brick.GetComponentsInChildren(typeof(Transform), true);
            foreach (Component c in components) {
                if (c.tag == "BrickHealth") {
                    if (c.GetComponent<TMP_Text>().text == "1") {
                        // If the brick has 1 health, destroy it
                        brickSpawnController.RemoveBrick(brick);
                    } else {
                        // If the brick has more than 1 health, reduce its health by 1
                        c.GetComponent<TMP_Text>().text = (int.Parse(c.GetComponent<TMP_Text>().text) - 1).ToString();
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "AddBall") {
            GameController gameController = FindObjectOfType<GameController>();
            gameController.AddNewBall();
            // Destroy the AddBall
            brickSpawnController.RemoveBrick(collision.gameObject);
        }
    }

    public void CheckForOutOfBounds() {
        if(transform.position.y < -5 || transform.position.y > 6 || transform.position.x < -4 || transform.position.x > 4) {
            // Return the ball to origin
            transform.position = new Vector3(0, -3.75f, 0);
            // Stop the ball
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            // Set the ball state to aim
            currentState = ballState.aim;
        }
    }
}