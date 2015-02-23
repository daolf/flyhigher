using UnityEngine;
using System.Collections;

public class PlanePhysics : MonoBehaviour {

	public int reactorForce;

	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		rotationSpeed = 1000;
	}

	void FixedUpdate() {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void decoller (float angle, float power) {
		rigidbody2D.MoveRotation (rigidbody2D.rotation + rotationSpeed * Time.deltaTime);
	
	}

}
