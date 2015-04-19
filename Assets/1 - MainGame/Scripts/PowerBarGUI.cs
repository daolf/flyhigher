using UnityEngine;
using System.Collections;

public class PowerBarGUI : MonoBehaviour {

	public float barValue = 0;
	//Vector2 pos = new Vector2(30, 300);
	Vector3 defaultSize;

	public enum State {Move, Stop};
	public State state = State.Move;
	public Transform redBar;
	public Transform whiteBG;
	
	private const float borneMax = 1.0f;
	private const float borneMin = 0.0f;
	private const float incrementSpeed = 2.5f;
	
	private bool up = true;


	void OnDisable() {
		Destroy(redBar.gameObject);
		Destroy (whiteBG.gameObject);
	}


	// Use this for initialization
	void Start () {
		defaultSize = redBar.transform.localScale;
	}

	void incremValue() {
		if (up){
			barValue += incrementSpeed * Time.deltaTime;
			if(barValue > borneMax) {
				barValue = borneMax;
				up = !up;
			}
		}
		else {
			barValue -= incrementSpeed * Time.deltaTime;
			if(barValue < borneMin) {
				barValue = borneMin;
				up = !up;
			}
		}
	}



	// Update is called once per frame
	void Update () {
		if (! GetComponentInParent<MainGame> ().isPause) {
			Bounds redBarBounds = redBar.GetComponent<Renderer> ().bounds;
			//redBarBounds.size = new Vector3(redBarBounds.size.x, defaultSize.y*barValue);
			redBar.transform.localScale = new Vector3 (redBar.transform.localScale.x, defaultSize.y * barValue);
			switch (state) {
			case State.Move:
				incremValue ();
				break;
			
			case State.Stop:
			
				break;
			}
		}
	}
}
