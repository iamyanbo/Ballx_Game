using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script will spawn bricks at the top of the screen
// and move them down to the ground
public class BrickSpawnController : MonoBehaviour {

    private GameObject[] bricks;
    private GameObject[] nonMovableBricks;
    private float[] brickXPositions = new float[6] { -2.33f, -1.4f, -0.466666667f, 0.466666667f, 1.4f, 2.33f};
    readonly float numberOfBricks = 6;
    private int addBallIndex;
    private LoadSavedGame loadSavedGame;
    private SaveGame saveGame;

    // Start is called before the first frame update
    void Start() {
        loadSavedGame = FindObjectOfType<LoadSavedGame>();
        saveGame = FindObjectOfType<SaveGame>();
        nonMovableBricks = GameObject.FindGameObjectsWithTag("NonMovableBrick");
        bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
        // Add all triangle bricks to the bricks array
        GameObject[] triangleBricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
        List<GameObject> brickList = new List<GameObject>(bricks);
        brickList.AddRange(triangleBricks);
        bricks = brickList.ToArray();
        GameData gameData = loadSavedGame.LoadGameData();
        if(gameData != null) {
            if(gameData.GetReturnToResumedGame()) {
                loadSavedGame.LoadGame();
            } else {
                SpawnBricks();
            }
        } else {
            SpawnBricks();
        }
        saveGame.SaveGameData();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SpawnBricks(int brickLevel = 1) {
        bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
        // Add all triangle bricks to the bricks array
        GameObject[] triangleBricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
        List<GameObject> brickList = new List<GameObject>(bricks);
        brickList.AddRange(triangleBricks);
        bricks = brickList.ToArray();
        // Add all the AddBall bricks to the bricks array
        GameObject[] addBallBricks = GameObject.FindGameObjectsWithTag("AddBall");
        brickList.AddRange(addBallBricks);
        bricks = brickList.ToArray();
        // Move all bricks down
        foreach(GameObject brick in bricks) {
            // Check the tag of the brick is brick and not nonmovablebrick
            if(brick.tag == "SquareBrick" || brick.tag == "AddBall" || brick.tag == "TriangleBrick") {
                brick.transform.position = new Vector3(brick.transform.position.x, brick.transform.position.y - 0.933333333f, 0);
            }        
        }
        if(brickLevel % 2 == 0) {
            addBallIndex = Random.Range(0, 6);
            // Spawn an AddBall brick at the addBallIndex
            GameObject newAddBall = Instantiate(GameObject.Find("AddBall"), new Vector3(brickXPositions[addBallIndex], 3.5f, 0), Quaternion.identity);
            newAddBall.tag = "AddBall";
        }
        bool brickSpawned = false;
        // Spawn 1-6 bricks at the top of the screen with text brickLevel with random orientation
        for(int i = 0; i < numberOfBricks; i++) {
            if(Random.Range(0, 2) == 1 && i != addBallIndex) {
                int brickIndex = Random.Range(0, 2);
                GameObject newBrick = Instantiate(nonMovableBricks[brickIndex], new Vector3(brickXPositions[i], 3.5f, 0), Quaternion.identity);
                // Rotate the brick between 0, 90, 180, 270 degrees
                int rotation = Random.Range(0, 4) * 90;
                newBrick.transform.Rotate(0, 0, rotation);
                // Get all components of the brick
                Component[] components = newBrick.GetComponentsInChildren(typeof(Transform), true);
                foreach(Component component in components) {
                    if(component.tag == "BrickHealth") {
                        component.GetComponent<TMP_Text>().text = brickLevel.ToString();
                        // Rotate the text back to 0 degrees
                        component.transform.Rotate(0, 0, -rotation);
                    }
                }
                if(brickIndex == 0) {
                    newBrick.tag = "SquareBrick";
                } else {
                    newBrick.tag = "TriangleBrick";
                }
                brickSpawned = true;
            }
        }
        // If no bricks were spawned, spawn a brick at a random position
        if(!brickSpawned) {
            int brickIndex = Random.Range(0, 2);
            // Pick random position for the brick that is not the addBallIndex
            int brickPosition = Random.Range(0, 6);
            while(brickPosition == addBallIndex) {
                brickPosition = Random.Range(0, 6);
            }
            GameObject newBrick = Instantiate(nonMovableBricks[brickIndex], new Vector3(brickXPositions[brickPosition], 3.5f, 0), Quaternion.identity);
            // Rotate the brick between 0, 90, 180, 270 degrees
            int rotation = Random.Range(0, 4) * 90;
            newBrick.transform.Rotate(0, 0, rotation);
            // Get all components of the brick
            Component[] components = newBrick.GetComponentsInChildren(typeof(Transform), true);
            foreach(Component component in components) {
                if(component.tag == "BrickHealth") {
                    component.GetComponent<TMP_Text>().text = brickLevel.ToString();
                    // Rotate the text back to 0 degrees
                    component.transform.Rotate(0, 0, -rotation);
                }
            }
            if(brickIndex == 0) {
                newBrick.tag = "SquareBrick";
            } else {
                newBrick.tag = "TriangleBrick";
            }
        }
        // Update the bricks array with square bricks
        bricks = GameObject.FindGameObjectsWithTag("SquareBrick");
        // Add TriangleBricks to the bricks array
        triangleBricks = GameObject.FindGameObjectsWithTag("TriangleBrick");
        brickList = new List<GameObject>(bricks);
        brickList.AddRange(triangleBricks);
        bricks = brickList.ToArray();
        // Add AddBall bricks to the bricks array
        addBallBricks = GameObject.FindGameObjectsWithTag("AddBall");
        brickList.AddRange(addBallBricks);
        bricks = brickList.ToArray();
    }

    public void RemoveBrick(GameObject newBrick) {
        // Remove the brick from the bricks array
        List<GameObject> brickList = new List<GameObject>(bricks);
        brickList.Remove(newBrick);
        bricks = brickList.ToArray();
        // Destroy the brick
        Destroy(newBrick);
    }
}
