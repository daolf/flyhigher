using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGame : MonoBehaviour {

	public GameObject plane;
	public GameObject guiComponent;
	public GameObject scoreContainer;
	public GameObject gui;
	public GameObject backgrounds;
	public Score guiScore;
	public Score guiBestScore;

	public enum State {INTRO,ANIM_LIFTOFF,MAIN,END_WIN,END_LOOSE};
	public State state;

	public IntroControl scriptIntroControl;
	public PlanePhysics scriptPlanePhysics;
	public IntroState scriptIntroState;
	public SlidingBackground scriptSlidingBackground;
	public RandomObject scriptRandomObject;
	public FuelControl scriptFuelControl;


	public float score;
	public float prevScore;

	public GameObject ground;

	public bool isPause;
	public float prevY;
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

		scriptSlidingBackground = backgrounds.GetComponent<SlidingBackground>();

		scriptRandomObject = this.GetComponent<RandomObject> ();
		scriptRandomObject.enabled = false;

		scriptFuelControl = GetComponent<FuelControl>();
		scriptFuelControl.enabled = false;


		prevY = scriptPlanePhysics.transform.position.y;
		isPause = false;
		guiBestScore.value = PlayerPrefs.GetInt (Constants.MAIN_GAME_HIGH_SCORE);

	}



	// Update is called once per frame
	void Update () {
		if (!isPause) {
			switch (state) {
			case State.INTRO:
			//If in the final state of introControl, leave the state
				if (scriptIntroControl.state == IntroControl.State.TWOCLICK) {
					scriptIntroState.enabled = false;
					scriptPlanePhysics.enabled = true;

					scriptPlanePhysics.decoller (scriptIntroControl.angle, scriptIntroControl.power);
					ground.GetComponent<SlidingBackground> ().swapGroundOnNextFrame ();
					scriptPlanePhysics.setOrigin ();
					state = State.ANIM_LIFTOFF;
				}
				break;
		
			case State.ANIM_LIFTOFF:
			//Plane lift off with informations gotten in Intro sequence

			//TODO do animation

				scriptIntroControl.enabled = false;
				scriptRandomObject.enabled = true;
				score = scriptPlanePhysics.getDistanceFromOrigin ();
				setScore (score);

			//On passe a l'état main quand on arrete de monter

				if (scriptPlanePhysics.transform.position.y < prevY) {
					state = State.MAIN;
				} else {
					prevY = scriptPlanePhysics.transform.position.y; 
				}

				break;
			case State.MAIN:

			//Wait for the fuel not to be consume with the first on touch
				scriptFuelControl.enabled = true;
			//On active l'intéraction fuel
				score = scriptPlanePhysics.getDistanceFromOrigin ();
				setScore (score);



			//FIN du jeu
				if (score == prevScore) {
					state = State.END_WIN;
				}
				break;
			case State.END_WIN:
				Application.LoadLevel ("EndMain");
				break;
			case State.END_LOOSE:
				break;
			}
		} 
	}

	void setScore(float score)
	{
		guiScore.value = (int)score;
		if (score > PlayerPrefs.GetInt (Constants.MAIN_GAME_HIGH_SCORE)) {
			guiBestScore.value = (int)score;
			PlayerPrefs.SetInt(Constants.MAIN_GAME_HIGH_SCORE,(int)score);
		}
	}

}
