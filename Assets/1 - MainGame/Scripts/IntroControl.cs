using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class IntroControl : MonoBehaviour {

	bool touched;
	public enum State {INIT, ONECLICK, TWOCLICK};

	public State state;

	public float power;
	public float powerMax;
	public float angle;
	public Component GUIObject;
	public PowerBarGUI powerBarGUIScript;
	public PivotGUI pivotGUIScript;

	void OnDisable() {
		powerBarGUIScript.enabled = false;
		pivotGUIScript.enabled = false;
	}

	void Start () {
		state = State.INIT;
		powerBarGUIScript = GUIObject.GetComponentInChildren<PowerBarGUI>();
		pivotGUIScript = GUIObject.GetComponentInChildren<PivotGUI>();
	}


	
	void Update () {
		if (! GetComponentInParent<MainGame> ().isPause && 
		    Input.GetMouseButtonDown (0) && 
		    !EventSystem.current.IsPointerOverGameObject()) {
			switch (state) {
			case State.INIT:
				power = powerBarGUIScript.barValue * powerMax;
				powerBarGUIScript.state = PowerBarGUI.State.stop;
				pivotGUIScript.state = PivotGUI.State.Move;
				state = State.ONECLICK;

				break;
			case State.ONECLICK:
				state = State.TWOCLICK;
				angle = pivotGUIScript.angle;
				pivotGUIScript.state = PivotGUI.State.Stop;
				break;
			case State.TWOCLICK:
				state = State.TWOCLICK;
				break;
			}
		} else {
			switch (state) {
			case State.INIT:
				powerBarGUIScript.state = PowerBarGUI.State.mov;
				pivotGUIScript.state = PivotGUI.State.Stop;
				break;
			case State.ONECLICK:
				powerBarGUIScript.state = PowerBarGUI.State.stop;
				pivotGUIScript.state = PivotGUI.State.Move;
				break;
			case State.TWOCLICK:
				powerBarGUIScript.state = PowerBarGUI.State.stop;
				pivotGUIScript.state = PivotGUI.State.Stop;
				break;
			}
		}
	}
}
