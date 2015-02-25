using UnityEngine;
using System.Collections;

public class MoveBackground : MonoBehaviour {
	
	// Ici on gère le déplacement du BackGround

	float speed;

	// Use this for initialization
	void Start () {
		speed = 1;
	}

	void FixedUpdate() {
		this.transform.Translate (Vector3.up * speed * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
