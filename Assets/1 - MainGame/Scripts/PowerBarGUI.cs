using UnityEngine;
using System.Collections;

public class PowerBarGUI : MonoBehaviour {

	public float barValue = 0;
	//Vector2 pos = new Vector2(30, 300);
	Vector3 defaultSize;

	public enum State {mov,stop};
	public State state = State.mov;
	public bool up = true;
	public float borneMax;
	public float borneMin;
	public float padding;
	public Transform redBar;
	public Transform whiteBG;


	void OnDisable() {
		print ("lola");
		Destroy(redBar.gameObject);
		Destroy (whiteBG.gameObject);
	}


	// Use this for initialization
	void Start () {
		defaultSize = redBar.transform.localScale;
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



	// Update is called once per frame
	void Update () {
		Bounds redBarBounds = redBar.GetComponent<Renderer>().bounds;
		//redBarBounds.size = new Vector3(redBarBounds.size.x, defaultSize.y*barValue);
		redBar.transform.localScale = new Vector3(redBar.transform.localScale.x, defaultSize.y*barValue);
		switch (state) {
		case State.mov:
			incremValue();
			break;
			
		case State.stop:
			
			break;
		}
	}
}
