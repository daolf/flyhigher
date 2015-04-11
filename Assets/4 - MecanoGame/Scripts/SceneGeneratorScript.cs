using UnityEngine;
using System.Collections;

public class SceneGeneratorScript : MonoBehaviour {

	// States of game
	enum State {Before, Tuto, StartRound, EndofRound, RestartRound, EndofGame };
	State state;

	public const int COGS_MAX_NB = 8;
	private int NbRealcogs;
	public int WIN_ROUND;
	public int LOST_ROUND; 
	public int WIN_SCORE; 
	public Score win_score;
	public Score myscore;

	public PrimaryCog[] cogs;
	public PrimaryCog cogToFind;

	public GameObject winRound;
	public GameObject lostRound;
	public GameObject looseMenu;
	public GameObject endMenu;

	public SpriteRenderer winBg;
	public SpriteRenderer lostBg;
	// end position of cog (the good one)
	public Transform cogFinalPosition;

	//Flag of the animation end
	public bool isAnimEnd;

	public TimeBarscript timeBar;
	public bool won;
	public bool check;
	public string scene;

	public Color normal;
	public Color win;
	public Color lost;
	public Color cogToFindColor;
	public Color goodCogColor;

	public GameObject cogsLevel1;
	public GameObject cogsLevel2;
	public GameObject cogsLevel3;
	private GameObject cogsLevel;

	// TODO put in prefab tuto
	public GameObject menupause;
	public GameObject role;
	public bool tuto;

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
		tuto = false; // for DEBUG
		isPause = false; //TODO verify 
		Time.timeScale = 1;
		initCogsLevel ();
		generateCogs ();
		// diplay the score to win
		win_score.value = WIN_SCORE;
		// set end of game
		timeBar.endCallback = endGameHandler;
	}

	// Use this for initialization
	void Start () {
		state = State.Before;
		tuto = MecanoLevelConfiguration.tuto;

		//TODO TODELETE
		if (role != null && tuto) {
			role.active = true;
		} else {
			tuto = false;
		}

		if (tuto) {
			state = State.Tuto;
			menupause.SetActive(false);
			isPause = true;//TODO semantique
		} else {
			state = State.Before;
			if (role != null) {
				role.active = false;
			} 
			menupause.SetActive(true);
			isPause = false;
			startRound ();
		}
	}

	// only useful at start to initalise the cogs with the right level
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
		// initialize all cogs with "random" ids (in fact each one need to be uniq, so its a shuffle)
		int[] cogIds = new int[cogs.Length];
		for (int i=0; i<cogs.Length; i++) {
			cogIds[i] = i;
		}
		// swap 20 times
		for(int step=0; step<20; step++) {
			int a = Random.Range(0, cogs.Length);
			int b = Random.Range(0, cogs.Length);
			int swap = cogIds[a];
			cogIds[a] = cogIds[b];
			cogIds[b] = swap;
		}
		
		// set cog ids and speed and color
		for(int i=0; i<cogs.Length; i++) {
			cogs[i].GetComponent<SpriteRenderer>().color = normal;
			cogs[i].setCogId(cogIds[i]);
			cogs[i].setSpeedRatio(3.0f);
			cogs[i].generator = this;
		}
	}

	// Start or ReStart Round
	void startRound () {
		state = State.StartRound;
		destroySmothTranslation ();
		winBg.enabled = false;
		lostBg.enabled = false;
		isAnimEnd = false;
		won = false;
		// replace cog
		generateCogs ();
		setAllSelectable ();
		isPause = false;
	}
	
	// Update is called once per frame
	void Update () {
		// end tuto
		// State: Tuto -> StartRound
		if (Input.GetButtonDown ("Fire1") ){
			getoutOfTuto();
		}
		// played click (cogSelected)
		// State: StartRound -> EndRound

		// annimation finished 
		// State: EndRound -> StartRound
		if (state == State.RestartRound) {
			startRound ();
		}
	}

	// called when a cog is selected by user
	public void cogSelected(PrimaryCog cog) {
		//State: StartRound -> EndRound
		state = State.EndofRound;
		isPause = true;
		if(cog.getCogId() == cogToFind.getCogId()) {

			// Update cogs 
			setGoodCogFind(cog);
			cog.GetComponent<SpriteRenderer>().color = win;

			// user won the round 
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

			// TODO mettre isSelectable a false pour tout les cogs
			//hasPlayed = true;
		}
	}
	
	public void hasWon(bool has) {
		state = State.EndofRound;
		isPause = true;
		won = has;
		setAllUnselectable();
		setScore (has);
		if (has) {
			winBg.enabled = true;
			StartCoroutine (fadOut (winRound));
		} else {
			lostBg.enabled = true;
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

	IEnumerator fadOut(GameObject menu ){
		menu.SetActive(true);
		yield return new WaitForSeconds (0.5f);
		//winRound.GetComponent<CanvasRenderer>().SetAlpha(0.5f); DELETE
		LeanTween.moveY(menu, 10000, 1.5f).setEase(LeanTweenType.easeInOutQuint);
		//LeanTween.alpha(menu,0f,1f).setEase(LeanTweenType.easeInOutQuint);  DELETE
		//TODO some where else menu.SetActive(false);
		yield return new WaitForSeconds (0.5f);
		menu.SetActive(false);
		yield return new WaitForSeconds (1f);
		state = State.RestartRound;	
	}

	// TODO put in prefab tuto
	void getoutOfTuto() {
			tuto = false;
		if (role != null) {
			role.active = tuto;
			menupause.SetActive(true);
		}
			isPause = tuto;
			//updateSceneRoudFinish ();
	}

	private void endGameHandler() {
		state = State.EndofGame;
		winRound.SetActive (false);
		lostRound.SetActive (false);
		setAllUnselectable();
		if (myscore.value >= WIN_SCORE) {
			//Game win 
			endMenu.GetComponent<Canvas> ().enabled = true;
		} else {
			//Game lost
			looseMenu.GetComponent<Canvas> ().enabled = true;
		}
	}

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

	public void setAllUnselectable () {
		cogToFind.setSelectable (false);
		for(int i=0; i<cogs.Length; i++) {
			cogs[i].setSelectable(false);
		}

	}

	public void destroySmothTranslation () {
		Destroy(cogToFind.gameObject.GetComponent("SmoothTranslation"));
		cogToFind.setSelectable (false);
		for(int i=0; i<cogs.Length; i++) {
			Destroy(cogs[i].gameObject.GetComponent("SmoothTranslation"));
		}
	}

	public void setAllSelectable () {
		cogToFind.setSelectable (false);
		for(int i=0; i<cogs.Length; i++) {
			cogs[i].setSelectable(true);
		}
		
	}

}
