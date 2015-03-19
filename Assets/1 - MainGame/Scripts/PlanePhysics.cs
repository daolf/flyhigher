using UnityEngine;
using System.Collections;

public class PlanePhysics : MonoBehaviour {

	public int reactorForce;
	private Vector2 origin;
	public float rotationSpeed;
	public Rigidbody2D rb;
	public float initialGravity;
	public Vector3 previousPos;
	private float bufferGravity;

	// Use this for initialization
	void Start () {
		origin = transform.position;
		initialGravity = (float)0.3;
		previousPos = transform.position;
	}

	void OnEnable() {
		GetComponent<Rigidbody2D>().gravityScale = 1;
	}

	void FixedUpdate() {

	}

	
	// Update is called once per frame
	void Update () {
		//Quand on appuie on alège la gravité
		if (Input.GetMouseButton(0)) {
			print ("TouchMainGame");
			//TODO rendre plus linéaire on alors on met un maximum sur y (rb.gravityscale)
			bufferGravity = (float)( 0.3 / -Mathf.Abs(rb.velocity.y));
			if (bufferGravity > -2 ) {
				rb.gravityScale = bufferGravity;
			}

		}
		//Si on appuie plus on la remet a la valeur initiale
		else {
			print ("NoTouch");
			rb.gravityScale = initialGravity;
		}

		//To face direction
		transform.right = rb.velocity;
		//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);;
	}
	

	public void decoller (float angle, float power) {
		Quaternion theRotation = transform.localRotation;
		theRotation.eulerAngles = new Vector3(0,0,angle);
		transform.localRotation = theRotation;	

		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		GetComponent<Rigidbody2D>().AddForce(dir*power*(float)0.5);

	}

	public void setOrigin() {
		origin = transform.position;
	}

	public float getDistanceFromOrigin() {
		return Vector2.Distance(transform.position, this.origin);
	}



}
