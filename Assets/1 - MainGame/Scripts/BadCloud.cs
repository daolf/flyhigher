using UnityEngine;
using System.Collections;

public class BadCloud : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D (Collider2D c) {
		PlanePhysics p = c.gameObject.GetComponent<PlanePhysics> ();
		if (p) {
			p.bonusState = PlanePhysics.BonusState.DOWNCLOUD;
		}
	}
	void OnTriggerExit2D (Collider2D c) {
		PlanePhysics p = c.gameObject.GetComponent<PlanePhysics> ();
		if (p) {
			p.bonusState = PlanePhysics.BonusState.NONE;
		}
	}
}