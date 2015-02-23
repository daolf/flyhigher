using UnityEngine;
using System.Collections;

public class PlanePhysicsIntro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlanePhysics physics = (PlanePhysics) GetComponent("PlanePhysics");

	
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
