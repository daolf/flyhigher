using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

	public Camera myCamera;
	public float to;
	public float from;
	
	public float duration = 2;
	private float elapsed = 0;
	
	//public float zoomVelocity = 0.8f;
	
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
			elapsed += Time.deltaTime;
			if (elapsed < duration) {
				myCamera.orthographicSize = from - (from - to) * easeSineRatio(elapsed/duration);
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
	
	private float easeSineRatio(float ratio) {
		return  (-0.5f) * (Mathf.Cos (ratio * Mathf.PI) - 1.0f);
	}
}
