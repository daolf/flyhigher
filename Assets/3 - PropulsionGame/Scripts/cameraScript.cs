using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

	public Camera myCamera;
	public float to ;
	
	public float zoomVelocity = 0.8f;
	
	// delegate called when the camera zoom finished
	public delegate void EventCallback();
	public EventCallback zoomFinishedCallback;
	
	private bool inZoom = true;
	

	void onEnabled() {
		to = myCamera.orthographicSize;
	}
	// Use this for initialization
	void Start () {
	}

	void FixedUpdate() {
		if(inZoom) {
			if (this.to < myCamera.orthographicSize) {
				myCamera.orthographicSize -= zoomVelocity * Time.deltaTime;
			}
			else {
				inZoom = false;
				zoomFinishedCallback();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
