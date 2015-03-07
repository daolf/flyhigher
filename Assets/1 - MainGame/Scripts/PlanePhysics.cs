using UnityEngine;
using System.Collections;

public class PlanePhysics : MonoBehaviour {

	public int reactorForce;
	private Vector2 origin;
	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		origin = transform.position;
	}

	void OnEnable() {
		GetComponent<Rigidbody2D>().gravityScale = 1;
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
		GetComponent<Rigidbody2D>().AddForce(dir*power);

	}

	public void setOrigin() {
		origin = transform.position;
	}

	public float getDistanceFromOrigin() {
		return Vector2.Distance(transform.position, this.origin);
	}

}
