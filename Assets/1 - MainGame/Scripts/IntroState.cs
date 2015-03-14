using UnityEngine;
using System.Collections;

public class IntroState : MonoBehaviour {

	public int introConstantSpeed;

	// Use this for initialization
	void Start () {
	//	rigidbody2D.velocity = Vector;
		GetComponent<Rigidbody2D>().AddForce (Vector2.right*introConstantSpeed);
		GetComponent<Rigidbody2D>().gravityScale = 0;
	}

	void OnDisable()
	{
		GetComponent<Rigidbody2D>().AddForce(-Vector2.right*introConstantSpeed);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
