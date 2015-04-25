using UnityEngine;
using System.Collections;

public class PivotGUI : MonoBehaviour {

	//public GameObject arrow;
	public Vector2 pos;
	public Vector2 size;
	public float angle;
	public enum State {Move, Stop};
	public State state = State.Move;
	public bool up=true;
	
	private const float borneSupAngle = 71.0f;
	private const float borneInfAngle = 3.0f;
	
	// angular speed (degrees per second)
	private const float angleSpeed = 80.0f;
	
	public GameObject arrowPivot;

	void OnDisable() {
		Destroy (this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}

	void incremAngle() {
		if (up){
			angle += angleSpeed * Time.deltaTime;
			
			if(angle > borneSupAngle) {
				angle = borneSupAngle - (angle - borneSupAngle);
				up = !up;
			}
		}
		else {
			angle -= angleSpeed * Time.deltaTime;
			
			if(angle < borneInfAngle) {
				angle = borneInfAngle + (borneInfAngle - angle);
				up = !up;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (! GetComponentInParent<MainGame> ().isPause) {
			arrowPivot.transform.eulerAngles = new Vector3 (0, 0, angle); 
			switch (state) {
			case State.Move:
				incremAngle ();
				break;
			
			case State.Stop:
				break;
			}
		}
	}
}
