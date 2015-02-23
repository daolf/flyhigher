using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour {

	public GameObject plane;

	public enum State {INTRO,ANIM_LIFTOFF,MAIN,END_WIN,END_LOOSE};
	public State state;

	public IntroControl scriptIntroControl;
	public PlanePhysics scriptPlanePhysics;


	// Use this for initialization
	void Start () {
		scriptIntroControl = this.GetComponent<IntroControl> ();
		scriptIntroControl.enabled = true;
		//intro script activation
		state = State.INTRO;

		scriptPlanePhysics = plane.GetComponent<PlanePhysics> ();


	}



	// Update is called once per frame
	void Update () {
		switch (state) {
		case State.INTRO :
			//If in the final state of introControl, leave the state
			if(scriptIntroControl.state == IntroControl.State.TWOCLICK) {

				state = State.ANIM_LIFTOFF;
			}
			break;
		
		case State.ANIM_LIFTOFF :
			//Plane lift off with informations gotten in Intro sequence
			scriptPlanePhysics.decoller(scriptIntroControl.angle,scriptIntroControl.power);


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
