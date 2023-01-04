using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumBallsDisplay : MonoBehaviour {

    private GameObject canvas;

    // Start is called before the first frame update
    void Start() {
        canvas = GameObject.Find("NumberBallsCanvas");
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateNumBalls(int numBalls) {
        canvas.GetComponentInChildren<TMP_Text>().text = numBalls.ToString();
    }
}
