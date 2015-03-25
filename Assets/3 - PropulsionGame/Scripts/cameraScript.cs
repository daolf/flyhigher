using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

	public Camera myCamera;
	public float to ;

	void onEnabled() {
		to = myCamera.orthographicSize;
	}
	// Use this for initialization
	void Start () {
	}

	void FixedUpdate() {
		if (this.to < myCamera.orthographicSize) {
			myCamera.orthographicSize -= 0.01f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
