using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour {

	public GameObject plane;

	public enum State {INTRO,ANIM_LIFTOFF,MAIN,END_WIN,END_LOOSE};
	public State state;

	public IntroControl scriptIntroControl;
	public PlanePhysics scriptPlanePhysics;
	public IntroState scriptIntroState;


	// Use this for initialization
	void Start () {
		scriptIntroControl = this.GetComponent<IntroControl> ();
		scriptIntroControl.enabled = true;

		scriptIntroState = plane.GetComponent<IntroState> ();
		scriptIntroState.enabled = true;
		//intro script activation
		state = State.INTRO;

		scriptPlanePhysics = plane.GetComponent<PlanePhysics> ();
		scriptPlanePhysics.enabled = false;


	}



	// Update is called once per frame
	void Update () {
		switch (state) {
		case State.INTRO :
			//If in the final state of introControl, leave the state
			if(scriptIntroControl.state == IntroControl.State.TWOCLICK) {
				scriptIntroState.enabled = false;
				scriptPlanePhysics.enabled = true;

				scriptPlanePhysics.decoller(scriptIntroControl.angle,scriptIntroControl.power);
				state = State.ANIM_LIFTOFF;
			}
			break;
		
		case State.ANIM_LIFTOFF :
			//Plane lift off with informations gotten in Intro sequence


			break;
		case State.MAIN :
			break;
		case State.END_WIN :
			break;
		case State.END_LOOSE :
			break;

		}
	}
}
