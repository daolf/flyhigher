using UnityEngine;
using System.Collections;

public class IntroState : MonoBehaviour {

	public int introConstantSpeed;

	// Use this for initialization
	void Start () {
	//	rigidbody2D.velocity = Vector;
		rigidbody2D.AddForce (Vector2.right*introConstantSpeed);
		rigidbody2D.gravityScale = 0;
	}

	void OnDisable()
	{
		rigidbody2D.AddForce(-Vector2.right*introConstantSpeed);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
