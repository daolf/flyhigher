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


	void Start () {
		state = State.INIT;
		powerBarGUIScript = GUIObject.GetComponent<PowerBarGUI>();
		pivotGUIScript = GUIObject.GetComponent<PivotGUI>();
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			switch (state) 
			{
			case State.INIT:
				state = State.ONECLICK;
				power = powerBarGUIScript.barValue;
				powerBarGUIScript.state = PowerBarGUI.State.stop;
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
