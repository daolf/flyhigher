using UnityEngine;
using System.Collections;

/**
 * Used to represent a Pipe element in the Propulsion Game.
 */
[RequireComponent (typeof(BoxCollider2D))]
public class PipeElementScript : MonoBehaviour {
	private PipeElement element = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown() {
		if(element != null) {
			element.orientation = element.orientation.rotateClockwise();
			gameObject.transform.localEulerAngles = new Vector3(0, 0, element.orientation.toDegrees());
		}
	}

	public void setPipeElement(PipeElement element) {
		this.element = element;
		// set orientation
		gameObject.transform.localEulerAngles = new Vector3(0, 0, element.orientation.toDegrees());
	}
}
