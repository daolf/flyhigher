using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGame : MonoBehaviour {

	public GameObject plane;
	public GameObject guiComponent;
	public GameObject scoreContainer;
	public GameObject gui;
	public GameObject endMenu;
	public GameObject backgrounds;
	public GameObject tutoPref;


	public Score guiScore;
	public Score guiBestScore;
	public Button bouttonPause;
	public CriticalPanelScript criticalPanel;

	public enum State {INTRO,ANIM_LIFTOFF,MAIN,END_WIN,END_LOOSE};
	public State state;

	public IntroControl scriptIntroControl;
	public PlanePhysics scriptPlanePhysics;
	public IntroState scriptIntroState;
	public SlidingBackground scriptSlidingBackground;
	public RandomObject scriptRandomObject;
	public FuelControl scriptFuelControl;

	public GenericTutoScript tutoScript;

	public float score;
	public float prevScore;

	public GameObject ground;


	public Scenario scenario;
	public int objectif;
	public bool isPause;
	public float prevY;
	// Use this for initialization
	void Start () {
		scenario = new Scenario ();
		objectif = scenario.getObjectif();
		bouttonPause.interactable = false;
		Time.timeScale = 1;

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
		scriptFuelControl.enabled = true;

		prevY = scriptPlanePhysics.transform.position.y;
		isPause = false;
		guiBestScore.value = PlayerPrefs.GetInt (Constants.MAIN_GAME_HIGH_SCORE);

		tutoScript = tutoPref.GetComponent<GenericTutoScript>();
		
		if (PlayerPrefs.GetInt (Constants.MAIN_GAME_ALREADY_PLAYED) == 0) {
			firstPlayTuto ();

			PlayerPrefs.SetInt (Constants.MAIN_GAME_ALREADY_PLAYED, 1);
		} else {
			displayObjectif();

		}
	}

	private void displayObjectif() {
		isPause = true;
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			string[] messages = new string[] {
				"L'objectif et de: "+objectif+" mètres."
			};
			tutoScript.say(messages);
		};

		tutoScript.outCallback = delegate() {
			isPause = false;
		};
		tutoScript.getIn();
	}

	private void displayWin() {
		isPause = true;
		tutoScript.setBubbleVisibility(false);
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			string message = "Bravo tu as réussi l'objectif, va voir du côté des jeux "+scenario.getGameUnlocked()+"";
			tutoScript.say(message);
		};

		tutoScript.outCallback = delegate() {
			Time.timeScale = 0;
			isPause = false;
			bouttonPause.image.enabled = false;
			bouttonPause.enabled = false;
			scenario.unlockNext();
			endMenu.GetComponent<Canvas>().enabled = true;
		};

		Time.timeScale = 1;
		tutoScript.getIn();
	}


	private void firstPlayTuto() {
		isPause = true;
		tutoScript.setBubbleVisibility(false);
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			
			string[] messages = new string[] {
				"Le but du jeu est de faire voler l'avion le plus loin possible.",
				"Pour cela, tu dois régler la jauge de puissance de propulsion de l'avion en touchant l\'écran au bon moment..",
				"Tu dois ensuite régler l'orientation au décollage de l'avion en touchant l\'écran au bon moment !",
				"Attention, des obstacles se glisseront dans ton parcours : Essaies d'éviter les montgolfières !",
				" Tu pourras aussi te servir des nuages d'air chaud qui relèvent ton avion, ainsi que les nuages d'air froid qui rabaissent ton avion.",
				"Pendant ton vol, n'oublies pas qu'en appuyant sur ton écran, tu peux propulser ton avion en brulant ton kérosène, désigné par la jauge verte en haut",
				"Essaies pour l'instant d'atteindre "+objectif+" mètres."
			};
			tutoScript.say(messages);
		};
		
		tutoScript.dialogueEndCallback = delegate() {
			tutoScript.dialogueEndCallback = null;
			tutoScript.setBubbleVisibility(false);
			tutoScript.getOut();
			isPause = false;
			tutoScript.hand.moveToWorldPosition(criticalPanel.transform.position, 1.8f);
			StartCoroutine(testCoroutine()); 
		};
		
		tutoScript.outCallback = delegate() {
			//isPause = false;
		};
		tutoScript.getIn();
	}

	private IEnumerator testCoroutine() {
		yield return new WaitForSeconds(2);
		tutoScript.hand.setHandKind(TutoHandScript.HandKind.HandClick);
		yield return new WaitForSeconds(0.3f);
		tutoScript.hand.setHandKind(TutoHandScript.HandKind.HandNormal);
		yield return new WaitForSeconds(0.5f);
		tutoScript.hand.setVisibility(false);
		tutoScript.getOut();
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (!isPause) {
			switch (state) {
			case State.INTRO:
				scriptFuelControl.enabled = false;

				//If in the final state of introControl, leave the state
				bouttonPause.interactable = true;

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

				//Si on s'arrête sur la piste d'aterrisage
				if (scriptPlanePhysics.rb.velocity.x <= 0.3) {
					state = State.END_LOOSE;
				}

			//On passe a l'état main quand on arrete de monter

				if (scriptPlanePhysics.transform.position.y < prevY) {
					state = State.MAIN;
				} else {
					prevY = scriptPlanePhysics.transform.position.y; 
				}

				break;
			case State.MAIN:
				if (Camera.main.WorldToScreenPoint(plane.transform.position)[1] > Screen.height) {
					criticalPanel.criticalState = CriticalPanelScript.CriticalState.CRITICAL;
				}
				else {
					criticalPanel.criticalState = CriticalPanelScript.CriticalState.NORMAL;
				}
			
				//Si on s'arrête sur la piste d'aterrisage
				if (scriptPlanePhysics.rb.velocity.x <= 0.3) {
					state = State.END_LOOSE;
				}

				//Wait for the fuel not to be consume with the first on touch
				scriptFuelControl.enabled = true;
				//On active l'intéraction fuel
				score = scriptPlanePhysics.getDistanceFromOrigin ();
				setScore (score);
				if (scriptPlanePhysics.myHeartBar.currLife == -1) {
					state = State.END_LOOSE;
				}

				break;
			case State.END_WIN:
				Application.LoadLevel ("EndMain");
				break;
			case State.END_LOOSE:
				//Fin du jeu
				Time.timeScale = 0;
				if (score > objectif ) {
					bouttonPause.interactable = false;
					displayWin();
				}
				else {
				bouttonPause.image.enabled = false;
				bouttonPause.enabled = false;
				endMenu.GetComponent<Canvas>().enabled = true;
				}
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
