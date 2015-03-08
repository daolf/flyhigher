using UnityEngine;
using System.Collections;

public class MoveBackground : MonoBehaviour {
	
	// Ici on gère le déplacement du BackGround

	public float speed;
	public float movement;
	public float previousX;
	public float maxMovement;
	// Use this for initialization
	void Start () {
		speed = 1;
		movement = 0;
		previousX = this.transform.position.x;
		// TODO modify calculation of maxMovement
		maxMovement = this.renderer.bounds.size.x - (float)10.5 ;
	}

	void FixedUpdate() {

		// We check we didn't get out off the screen
		// Check if not out of the screen
		if (movement < maxMovement) {
		previousX = this.transform.position.x;
		this.transform.Translate (Vector3.up * speed * Time.deltaTime);
		movement += (this.transform.position.x - previousX);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
