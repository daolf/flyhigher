using UnityEngine;
using System.Collections;

public class SlidingBackground : MonoBehaviour {

	public GameObject leftBackground, rightBackground;
	public GameObject mainCamera;
	public float xFactor;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float cameraX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(mainCamera.GetComponent<Camera>().rect.min).x;
		Vector3 tmpPos;
		if(leftBackground.GetComponent<Renderer>().bounds.max.x < cameraX)
		{
			tmpPos = rightBackground.transform.position;
			tmpPos.x += rightBackground.GetComponent<Renderer>().bounds.size.x;
			leftBackground.transform.position = tmpPos;

			GameObject tmp = leftBackground;
			leftBackground = rightBackground;
			rightBackground = tmp;
		}
	}
}
