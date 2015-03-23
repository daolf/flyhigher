using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomObject : MonoBehaviour {

	List<Transform> objects = new List<Transform>();


	public Transform balloonPrefab;
	public Transform goodCloudPrefab;
	public Transform badCloudPrefab;
	public float _xMin, _xMax, _yMin, _yMax;
	public GameObject skyBG;
	public GameObject rightBG;
	public GameObject plane;
	public float lastPos;
	// Use this for initialization
	void Start () {

	}

	void OnEnable()
	{
		rightBG = skyBG.GetComponent<SlidingBackground>().rightBackground;
		_yMax = rightBG.GetComponent<Renderer>().bounds.max.y;
		_yMin = _yMax - rightBG.GetComponent<Renderer>().bounds.size.y*2/3;
		lastPos = plane.transform.position.x;
		
	}

	void OnDisable() {
		stopCreateRandomBalloon();
		for (int i = 0; i < objects.Count; i++) {
			Transform balloon = objects[i];
			if(balloon != null) {
				Destroy(balloon.gameObject);
			}
		}
		objects.RemoveRange(0, objects.Count);
	}

	// Update is called once per fram
	void Update () {
		rightBG = skyBG.GetComponent<SlidingBackground>().rightBackground;
		_xMin = rightBG.GetComponent<Renderer>().bounds.max.x;
		_xMax = _xMin*2;

		//Lower value if we want more object
		if ( (plane.transform.position.x - lastPos) > 7 ) {
			createRandomObject(_xMin);
			lastPos = plane.transform.position.x;
		}
	}
	


	void stopCreateRandomBalloon()
	{
		CancelInvoke();
	}

	void createRandomObject(float x) {
		float y = Random.Range(_yMin, _yMax);
		float paddleX = Random.Range (0,5F);
		float myX = x + paddleX;
		float r = Random.Range (0F, 1F);


		//print ("r= "+ r + "x = " + x + " paddleX = " + paddleX);
		if (r < 0.25) {
			//print ("pop BallonPrefab");
			objects.Add ((Transform)Instantiate (balloonPrefab, new Vector3 (myX, y, -2), Quaternion.identity));
		} else if (r < 0.5) {
			//print ("pop GoodCloud");
			objects.Add ((Transform)Instantiate (goodCloudPrefab, new Vector3 (myX, y, -2), Quaternion.identity));
		} else if (r < 0.75) {
			//print ("pop BadBloud");
			objects.Add ((Transform)Instantiate (badCloudPrefab, new Vector3 (myX, y, -2), Quaternion.identity));
		} else {
			//nothing
			//print ("no pop");
		}
	}
}
