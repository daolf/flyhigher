using UnityEngine;
using System.Collections;

public class GroundTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D c) {
		PlanePhysics p = c.gameObject.GetComponent<PlanePhysics> ();
		
		if (p) {
			p.GetComponentInParent<MainGame>().state = MainGame.State.END_LOOSE;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
