using UnityEngine;
using System.Collections;

public class PlanePhysicsIntro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlanePhysics physics = (PlanePhysics) GetComponent("PlanePhysics");

		// physics.angle = transform.rotation.eulerAngles.z;
		Vector3 dir = Quaternion.AngleAxis(physics.angle, Vector3.forward) * Vector3.right;
		rigidbody2D.AddForce(dir*physics.reactorForce);
	}
	
	// Update is called once per frame
	void Update () {
	}
	void FixedUpdate()
	{
		print (transform.rotation.eulerAngles);
		// physics.angle = transform.rotation[3];
	}
}
