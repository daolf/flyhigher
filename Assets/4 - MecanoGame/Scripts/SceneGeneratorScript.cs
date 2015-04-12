using UnityEngine;
using System.Collections;

public class SceneGeneratorScript : MonoBehaviour {
	
	// States of game
	enum State {Before, Tuto, StartRound, EndofRound, RestartRound, EndofGame };
	State state;

	// Cogs
	public const int COGS_MAX_NB = 8;
	private int NbRealcogs;
	public PrimaryCog[] cogs;
	public PrimaryCog cogToFind;

	// Players 
	public int WIN_ROUND;
	public int LOST_ROUND; 
	public int WIN_SCORE; 
	public Score win_score;
	public Score myscore;
	public Score scoreLostRound;
	public Score scoreWinRound;

	// Menus
	public GameObject winRound;
	public GameObject lostRound;
	public GameObject looseMenu;
	public GameObject endMenu;
	public TimeBarscript timeBar;
	public string scene;

	// End position of cog (the good one)
	public Transform cogFinalPosition;
	public SpriteRenderer winBg;
	public SpriteRenderer lostBg;

	// Flag of the animation end
	public bool isAnimEnd; // TODO DELETE ?


	// Colors
	public Color normal;
	public Color win;
	public Color lost;
	public Color cogToFindColor;
	public Color goodCogColor;

	// Levels
	public GameObject cogsLevel1;
	public GameObject cogsLevel2;
	public GameObject cogsLevel3;
	private GameObject cogsLevel;

	// Tutorial
	public GenericTutoScript tutoScript;
	public GameObject menupause;
	public bool tuto;
	// TODO put in a commun file
	private string[] dilogue1 = new string[]
	{"Bonjour, je suis Yassine, et je suis mécanicien" +
		"de maintenance aéronautique. Tout les jours, " +
		"je vérifie que tout les éléments servant à faire",
		"voler l'avion sont en bon état."
	};
	private string[] dilogue2 = new string[] 
	{"Dans mon métier, je mène des essais d'entretien " +
		"et je recherche les pannes. A la moindre anomalie, " +
		"je suis chargé du remplacement",
		" de la pièce defectueuse. " +
		"La rigueur est indispensable car la sécurité des" +
		"passagers dépend de moi."
	};
	private string[] dilogue3 = new string[] {};

	private bool m_isPause;

	public bool isPause {
		get {
			return m_isPause;
		}
		set {
			m_isPause = value;
			timeBar.activated = !value;
		}
	}

	void Awake() {
		state = State.Before;
		// Default
		tuto = true; 
		isPause = false;
		Time.timeScale = 1;
		initCogsLevel ();
		generateCogs ();
		// set all score
		win_score.value = WIN_SCORE;
		scoreWinRound.value = WIN_ROUND ; 
		scoreLostRound.value = LOST_ROUND;
		// set end of game
		timeBar.endCallback = endGameHandler;
	}

	// Use this for initialization.
	void Start () {
		state = State.Before;
		tuto = MecanoLevelConfiguration.tuto;
		winBg.enabled = false;
		lostBg.enabled = false;

		if (tuto) {
			state = State.Tuto;
			menupause.SetActive(false);
			isPause = true;
			switch (MecanoLevelConfiguration.level) {
				case 1: 
					firstPlayTuto();
					break;
				case 2:
					secondPlayTuto();
					break;
				case 3:
					firstPlayTuto();
					break;
				default:
					firstPlayTuto();
					break;
			}
		} else {
			state = State.Before;
			menupause.SetActive(true);
			isPause = false;
			startRound ();
		}
	}

	// First tutorial.
	private void firstPlayTuto() {
		isPause = true;
		tutoScript.setBubbleVisibility(false);
		
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			tutoScript.say(dilogue1[0]);
		};
		
		tutoScript.outCallback = delegate() {
			isPause = false;
			menupause.SetActive(true);
			tuto = false;
		};
		
		tutoScript.getIn();
	}

	// Second tutorial.
	private void secondPlayTuto() {
		isPause = true;
		tutoScript.setBubbleVisibility(false);
		
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			tutoScript.say(dilogue2[0]);
		};
		
		tutoScript.outCallback = delegate() {
			isPause = false;
			menupause.SetActive(true);
			tuto = false;
		};
		
		tutoScript.getIn();
	}


	// Only useful at start to initalise the cogs with the right level.
	private void initCogsLevel () {
		state = State.Before;
		switch (MecanoLevelConfiguration.level) {
		case 1: 
			cogsLevel = cogsLevel1;
			break;
		case 2:
			cogsLevel = cogsLevel2;
			break;
		case 3:
			cogsLevel = cogsLevel3;
			break;
		default:
			cogsLevel = cogsLevel1;
			break;
		}
		cogsLevel.SetActive(true);
		NbRealcogs = cogsLevel.transform.childCount;
		cogs = new PrimaryCog[NbRealcogs];
		if(cogsLevel != null) {
			int i = 0;
			foreach (Transform child in cogsLevel.transform) {
				cogs[i] = child.GetComponent<PrimaryCog>();
				i++;
			}
		}
	}
	
	public void generateCogs () {
		cogToFind.setCogId(Random.Range(0, cogs.Length));
		cogToFind.GetComponent<SpriteRenderer>().color = cogToFindColor;
		// Initialize all cogs with "random" ids 
		// (in fact each one need to be uniq, so its a shuffle).
		int[] cogIds = new int[cogs.Length];
		for (int i=0; i<cogs.Length; i++) {
			cogIds[i] = i;
		}

		// Swap 20 times.
		for(int step=0; step<20; step++) {
			int a = Random.Range(0, cogs.Length);
			int b = Random.Range(0, cogs.Length);
			int swap = cogIds[a];
			cogIds[a] = cogIds[b];
			cogIds[b] = swap;
		}
		
		// Set cog ids and speed and color.
		for(int i=0; i<cogs.Length; i++) {
			cogs[i].GetComponent<SpriteRenderer>().color = normal;
			cogs[i].setCogId(cogIds[i]);
			cogs[i].setSpeedRatio(3.0f);
			cogs[i].generator = this;
		}
	}

	// Start or ReStart Round.
	void startRound () {
		state = State.StartRound;
		destroySmothTranslation ();
		winBg.enabled = false;
		lostBg.enabled = false;
		isAnimEnd = false;
		generateCogs ();
		setAllSelectable ();
		isPause = false;
	}
	
	// Update is called once per frame.
	void Update () {

		// Played click handeled in the PrimaryCog.cs (cogSelected)
		// State: StartRound -> EndRound

		// Annimation finished.
		// State: EndRound -> StartRound
		if (state == State.RestartRound ) {
			startRound ();
		}
	}

	// Called when a cog is selected by user.
	public void cogSelected(PrimaryCog cog) {
		// State: StartRound -> EndRound
		state = State.EndofRound;
		isPause = true;
		if(cog.getCogId() == cogToFind.getCogId()) {

			// Update cogs 
			setGoodCogFind(cog);
			cog.GetComponent<SpriteRenderer>().color = win;

			// User won the round 
			hasWon(true);

		} else { 

			// Update cogs 
			PrimaryCog goodOne = null;
			for (int i=0; i<cogs.Length; i++) {
				if(cogs[i].getCogId() == cogToFind.getCogId()) {
					goodOne = cogs[i];
				}
			}
			if(goodOne) {
				setGoodCogFind(goodOne);
				goodOne.GetComponent<SpriteRenderer>().color = goodCogColor;
			}
			cog.GetComponent<SpriteRenderer>().color = lost;

			// user lost the round 
			hasWon(false);

		}
	}

	// Actions after the user has finished a round.
	public void hasWon(bool has) {
		state = State.EndofRound;
		isPause = true;
		setAllUnselectable();
		setScore (has);
		if (has) {
			winBg.enabled = true;
			StartCoroutine (showScore(scoreWinRound.gameObject));
			StartCoroutine (fadOut (winRound));

		} else {
			lostBg.enabled = true;
			StartCoroutine (showScore(scoreLostRound.gameObject));
			StartCoroutine (fadOut (lostRound));
		}

	}
	
	public void setScore(bool has) {
		if (has) {
			myscore.value += WIN_ROUND;
		} else {
			if (myscore.value > LOST_ROUND) {
				myscore.value -= LOST_ROUND;
			} else {
				myscore.value = 0;
			}
		}
	}

	// Annimation after the user has selected a cog.
	IEnumerator fadOut(GameObject menu ) {
		isPause = true;

		// Scale
		Vector3 oldscale = menu.transform.localScale;
		menu.SetActive(true);
		Vector3 scale = new Vector3 (oldscale.x*1.2f,oldscale.y*1.2f,oldscale.z*1.2f);
		LeanTween.scale (menu,scale,0.2f);
		yield return new WaitForSeconds (0.5f);

		// Position
		Vector3 oldPos = menu.transform.position;
		LeanTween.moveY(menu, 900, 0.9f).setEase(LeanTweenType.easeInOutQuint);

		yield return new WaitForSeconds (0.5f);
		for (int i=0; i<cogs.Length; i++) {
			cogs [i].GetComponent<SpriteRenderer> ().color = normal;
		}
		yield return new WaitForSeconds (0.5f);

		// Reset
		menu.SetActive(false);
		menu.transform.localScale= oldscale;
		menu.transform.position = oldPos;
		state = State.RestartRound;	
	}

	// Annimation that showes if the evolution of the score
	IEnumerator showScore (GameObject score) {

		// Scale
		Vector3 oldscale = score.transform.localScale;
		score.SetActive(true);
		Vector3 scale = new Vector3 (oldscale.x*1.2f,oldscale.y*1.2f,oldscale.z*1.2f);
		LeanTween.scale (score,scale,0.2f);

		// Position TODO
		/*
		Vector3 oldPos = score.transform.position;
		LeanTween.moveX(score, -5, 0.7f).setEase(LeanTweenType.easeInOutQuint);
		LeanTween.moveY(score, -5, 0.7f).setEase(LeanTweenType.easeInOutQuint);
		*/
		yield return new WaitForSeconds (0.5f);

		// Reset
		score.SetActive(false);
		score.transform.localScale= oldscale;
		//score.transform.position = oldPos;
	}
	

	// Called at the end of the game.
	private void endGameHandler() {
		state = State.EndofGame;
		winRound.SetActive (false);
		lostRound.SetActive (false);
		setAllUnselectable();
		if (myscore.value >= WIN_SCORE) {
			// Game win 
			endMenu.GetComponent<Canvas> ().enabled = true;
		} else {
			// Game lost
			looseMenu.GetComponent<Canvas> ().enabled = true;
		}
	}

	// Applise a smooth transition to the goodOne. 
	private void setGoodCogFind(PrimaryCog goodOne) {
		goodOne.setSelectable (false);
		goodOne.gameObject.AddComponent<SmoothTranslation> ();
		SmoothTranslation st = goodOne.GetComponent<SmoothTranslation> ();
		st.sceneGenerator = this;
		st.duration = 1;
		st.from = goodOne.transform.position;
		st.to = cogFinalPosition.position;
	}

	public PrimaryCog getCogToFind() {
		return cogToFind;
	}

	// Make all cogs clickable for the user.
	public void setAllSelectable () {
		cogToFind.setSelectable (false);
		for(int i=0; i<cogs.Length; i++) {
			cogs[i].setSelectable(true);
		}
	}

	// Make all cogs unclickable for the user.
	public void setAllUnselectable () {
		cogToFind.setSelectable (false);
		for(int i=0; i<cogs.Length; i++) {
			cogs[i].setSelectable(false);
		}

	}

	// Puts the cogs in there original position.
	public void destroySmothTranslation () {
		Destroy(cogToFind.gameObject.GetComponent("SmoothTranslation"));
		cogToFind.setSelectable (false);
		for(int i=0; i<cogs.Length; i++) {
			Destroy(cogs[i].gameObject.GetComponent("SmoothTranslation"));
		}
	}
		
}
