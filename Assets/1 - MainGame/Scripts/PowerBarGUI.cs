using UnityEngine;
using System.Collections;

public class PowerBarGUI : MonoBehaviour {

	public float barValue = 0;
	Vector2 pos = new Vector2(30, 300);
	Vector2 size = new Vector2(30, 100);
	Texture2D progressBarEmpty;
	Texture2D progressBarFull;

	public enum State {mov,stop};
	public State state = State.mov;
	public bool up=true;
	public float borneMax;
	public float borneMin;
	public float padding;




	// Use this for initialization
	void Start () {
		progressBarEmpty = new Texture2D(1,1);
		progressBarEmpty.SetPixel(0,0,Color.blue);
		progressBarEmpty.Apply();

		progressBarFull = new Texture2D(1,1);
		progressBarFull.SetPixel(0,0,Color.magenta);
		progressBarFull.Apply();

		borneMax= 1;
		borneMin= 0;
		padding=0.02F;

		//barDisplay = 100f; 
	}

	void incremValue() {
		
		if (up){
			if(barValue<borneMax) 
				barValue += padding;
			else 
				up = !up; 	
		}
		else {
			if(barValue>borneMin) 
				barValue -= padding;
			else 
				up = !up; 
		}
	}


	void OnGUI() {
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.DrawTexture (new Rect (0,0, size.x, size.y), progressBarEmpty);
		GUI.BeginGroup (new Rect (0, size.y*(1-barValue), size.x, size.y));
		GUI.DrawTexture (new Rect (0,0, size.x, size.y), progressBarFull);
		GUI.EndGroup ();
		GUI.EndGroup ();
		

		switch (state) {
		case State.mov:
			incremValue();
			break;
			
		case State.stop:
			
			break;
		}

	}



	// Update is called once per frame
	void Update () {
	
	}
}
