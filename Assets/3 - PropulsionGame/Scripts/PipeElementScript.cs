using UnityEngine;
using System.Collections;

/**
 * Used to represent a Pipe element in the Propulsion Game.
 */
[RequireComponent (typeof(BoxCollider2D))]
public class PipeElementScript : MonoBehaviour {
	// prefab used to display win path (yes... as a public attribute...)
	public Transform winPathIndicator;

	private PipeElement element = null;
	
	// for smooth rotation
	private const float rotationSpeed = 720;
	private bool inSmoothRotation = false;
	private float targetedAngle;
	
	// state for activation on touch
	private bool touchEnable = true;
	
	// for smooth color transition (temp?)
	private bool inSmoothFading = false;
	private Color fadingColorIn = new Color(1, 1, 1, 0);
	public Color fadingColorOut;
	
	private PipesGeneratorScript generator;

	float fadingElapsed = 0;
	
	// Use this for initialization
	void Start () {
		generator = GameObject.Find ("Pipes Generator").GetComponent<PipesGeneratorScript>();
	}
	
	public void setTouchEnable(bool val) {
		touchEnable = val;
	}
	
	// Update is called once per frame
	void Update () {
		if(inSmoothRotation) {
			float current = transform.localEulerAngles.z;
			// TODO floating point == is not reliable, use abs(x)<epsilon
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
		
		if(inSmoothFading) {
			fadingElapsed += Time.deltaTime;
			transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(fadingColorIn, fadingColorOut, fadingElapsed/0.75f);

			if(fadingElapsed > 0.75f)
				inSmoothFading = false;
		}
	}
	
	private void smoothRotate(float angle) {
		targetedAngle = angle;
		inSmoothRotation = true;
	}

	void OnMouseDown() {
		if(element != null && touchEnable && !generator.isPause)
		{
			rotateClockwiseOnce();
		}
	}
	
	public void rotateClockwiseOnce() {
		element.orientation = element.orientation.rotateClockwise();
		smoothRotate(element.orientation.toDegrees());
		generator.mainAudio.Play();
	}

	public void setPipeElement(PipeElement element) {
		this.element = element;
		// set orientation
		gameObject.transform.localEulerAngles = new Vector3(0, 0, element.orientation.toDegrees());
	}
	
	public void setWinPath(bool winPath) {
		if(winPath) {
			/*Transform indicator = Instantiate(winPathIndicator).transform;
			indicator.parent = transform;
			indicator.localPosition = new Vector3(0, -0.3f, 0);*/
			
			// for now, just change color of the pipe (delayed)
			//GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.black, 0.3f);
			fadingElapsed = 0;
			inSmoothFading = true;
		}
	}
}
