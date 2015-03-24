using UnityEngine;
using System.Collections;

public class SlidingForeground : MonoBehaviour {
	
	public GameObject leftForeground, mid2Foreground, mid1Foreground, rightForeground;
	public GameObject mainCamera;
	public float xFactor;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float cameraX = mainCamera.GetComponent<Camera> ().ScreenToWorldPoint (mainCamera.GetComponent<Camera> ().rect.max).x + leftForeground.GetComponent<Renderer> ().bounds.size.y*(float)2;
		Vector3 tmpPos;
		if(rightForeground.GetComponent<Renderer>().bounds.min.x > cameraX)
		{
			tmpPos = leftForeground.transform.position;
			tmpPos.x -= leftForeground.GetComponent<Renderer>().bounds.size.x;
			rightForeground.transform.position = tmpPos;
			
			GameObject tmp = rightForeground;
			rightForeground = mid2Foreground;
			mid2Foreground = mid1Foreground;
			mid1Foreground = leftForeground;
			leftForeground = tmp;
		}
	}

}
