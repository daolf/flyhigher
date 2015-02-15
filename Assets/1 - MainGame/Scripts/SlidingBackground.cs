using UnityEngine;
using System.Collections;

public class SlidingBackground : MonoBehaviour {

	GameObject leftBackground, rightBackground;
	GameObject mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find ("MainCamera");
		leftBackground = GameObject.Find ("background1");
		rightBackground = GameObject.Find ("background2");

	}
	
	// Update is called once per frame
	void Update () {
		if (mainCamera.transform.position.x >= rightBackground.transform.position.x) {
			Vector2 tmpPos = leftBackground.transform.position;
			tmpPos.x += (rightBackground.transform.position.x-leftBackground.transform.position.x)*2;
			leftBackground.transform.position = tmpPos;

			GameObject tmp = leftBackground;
			leftBackground = rightBackground;
			rightBackground = tmp;
		}
	}
}
