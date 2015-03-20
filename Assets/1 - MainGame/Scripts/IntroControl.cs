using UnityEngine;
using System.Collections;

public class IntroControl : MonoBehaviour {

	bool touched;
	public enum State {INIT, ONECLICK, TWOCLICK};

	public State state;

	public float power;
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
		if(Input.GetMouseButtonDown(0))
		{
			switch (state) 
			{
			case State.INIT:
				state = State.ONECLICK;
				power = powerBarGUIScript.barValue*1000;
				powerBarGUIScript.state = PowerBarGUI.State.stop;
				pivotGUIScript.state = PivotGUI.State.mov;

				break;
			case State.ONECLICK:
				state = State.TWOCLICK;
				angle = pivotGUIScript.angle;
				pivotGUIScript.state = PivotGUI.State.stop;
				break;
			case State.TWOCLICK:
				state = State.TWOCLICK;
					break;
			}
		}
	}
}
