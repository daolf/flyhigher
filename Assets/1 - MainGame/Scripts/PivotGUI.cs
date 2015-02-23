using UnityEngine;
using System.Collections;

public class PivotGUI : MonoBehaviour {

	public Texture2D arrowTexture;
	public Texture2D angleTexture;
	public Vector2 pos;
	public Vector2 size;
	public float angle;
	public enum State {mov,stop};
	public State state = State.mov;
	public bool up=true;
	public float borneSupAngle;
	public float borneInfAngle;
	public float pasAngle;

	// Use this for initialization
	void Start () {
		borneSupAngle= 80;
		borneInfAngle= 0;
		pasAngle=0.87F;
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

	void OnGUI() {


		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		// BG
		GUI.DrawTexture(new Rect(0, 0, size.x, size.y), angleTexture);
		// ARROW
		GUIUtility.RotateAroundPivot(-angle, new Vector2(0,size.y));

		GUI.DrawTexture(new Rect(size.x-17, size.y-12, 17, 12), arrowTexture);

		GUI.EndGroup();

		switch (state) {
		case State.mov:
			incremAngle();
			break;

		case State.stop:

			break;
		}



	}

	// Update is called once per frame
	void Update () {
	}
}
