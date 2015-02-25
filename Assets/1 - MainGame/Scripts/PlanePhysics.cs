using UnityEngine;
using System.Collections;

public class PlanePhysics : MonoBehaviour {

	public int reactorForce;

	public float rotationSpeed;

	// Use this for initialization
	void Start () {
	}

	void OnEnable() {
		rigidbody2D.gravityScale = 1;
	}

	void FixedUpdate() {

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void decoller (float angle, float power) {
		Quaternion theRotation = transform.localRotation;
		theRotation.eulerAngles = new Vector3(0,0,angle);
		transform.localRotation = theRotation;	

		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		rigidbody2D.AddForce(dir*power);

	}

}
