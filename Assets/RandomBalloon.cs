using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomBalloon : MonoBehaviour {

	List<Transform> balloons = new List<Transform>();


	public Transform balloonPrefab;
	float _xMin, _xMax, _yMin, _yMax;
	public GameObject skyBG;
	GameObject rightBG;
	// Use this for initialization
	void Start () {

	}

	void OnEnable()
	{
		rightBG = skyBG.GetComponent<SlidingBackground>().rightBackground;
		_yMax = rightBG.GetComponent<Renderer>().bounds.max.y;
		_yMin = _yMax - rightBG.GetComponent<Renderer>().bounds.size.y*2/3;
		
		startCreateRandomBalloon();
	}

	void OnDisable() {
		stopCreateRandomBalloon();
		for (int i = 0; i < balloons.Count; i++) {
			Transform balloon = balloons[i];
			if(balloon != null) {
				Destroy(balloon.gameObject);
			}
		}
		balloons.RemoveRange(0, balloons.Count);
	}

	// Update is called once per frame
	void Update () {
		rightBG = skyBG.GetComponent<SlidingBackground>().rightBackground;
		_xMin = rightBG.GetComponent<Renderer>().bounds.max.x;
		_xMax = _xMin*2;
	}
	
	void startCreateRandomBalloon()
	{
		InvokeRepeating("createRandomBalloon", 0, 2F);
	}

	void stopCreateRandomBalloon()
	{
		CancelInvoke();
	}

	void createRandomBalloon() {
		float x = Random.Range(_xMin, _xMax);
		float y = Random.Range(_yMin, _yMax);
		balloons.Add((Transform)Instantiate(balloonPrefab,new Vector3(x, y, -2), Quaternion.identity));
	}
}
