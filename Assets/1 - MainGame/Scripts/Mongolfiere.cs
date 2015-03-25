using UnityEngine;
using System.Collections;

public class Mongolfiere : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D c) {
		PlanePhysics p = c.gameObject.GetComponent<PlanePhysics> ();
		if (p) {
			p.handleMongolfiere();
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
