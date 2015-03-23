using UnityEngine;
using System.Collections;

/**
 * Used to represent a Pipe element in the Propulsion Game.
 */
[RequireComponent (typeof(BoxCollider2D))]
public class PipeElementScript : MonoBehaviour {
	private PipeElement element = null;
	
	private float rotationSpeed = 360;
	
	private bool inSmoothRotation = false;
	private float targetedAngle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(inSmoothRotation) {
			float current = transform.localEulerAngles.z;
			if(current == targetedAngle) {
				inSmoothRotation = false;
			}
			else {
				float delta = targetedAngle - current;
				float wise = 1;
				delta = delta < 0 ? 360.0f + delta : delta;
				
				if(delta > 180) {
					delta = 360.0f - delta;
					wise = -1;
				}
				
				float realDelta = rotationSpeed * Time.deltaTime;
				realDelta = realDelta > delta ? delta : realDelta;
				
				transform.localEulerAngles =  new Vector3(0, 0, current + realDelta * wise);
			}
		}
	}
	
	private void smoothRotate(float angle) {
		targetedAngle = angle;
		inSmoothRotation = true;
	}

	void OnMouseDown() {
		if(element != null) {
			element.orientation = element.orientation.rotateClockwise();
			smoothRotate(element.orientation.toDegrees());
		}
	}

	public void setPipeElement(PipeElement element) {
		this.element = element;
		// set orientation
		gameObject.transform.localEulerAngles = new Vector3(0, 0, element.orientation.toDegrees());
	}
}
