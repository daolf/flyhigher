using UnityEngine;
using System.Collections;

public class PivotGUI : MonoBehaviour {

	//public GameObject arrow;
	public Vector2 pos;
	public Vector2 size;
	public float angle;
	public enum State {mov,stop};
	public State state = State.mov;
	public bool up=true;
	public float borneSupAngle;
	public float borneInfAngle;
	public float pasAngle;
	public GameObject arrowPivot;

	void OnDisable() {
		Destroy (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		borneSupAngle= 71;
		borneInfAngle= 0;
		pasAngle=2;
	}

	void incremAngle() {
	
		if (up){
			if(angle<borneSupAngle) 
				angle += pasAngle;
			else 
				up = !up; 	
		}
		else {
			if(angle>borneInfAngle) 
				angle -= pasAngle;
			else 
				up = !up; 
		}
	}

	// Update is called once per frame
	void Update () {
		arrowPivot.transform.eulerAngles = new Vector3(0, 0, angle); 
		switch (state) {
		case State.mov:
			incremAngle();
			break;
			
		case State.stop:
			break;
		}

	}
}
