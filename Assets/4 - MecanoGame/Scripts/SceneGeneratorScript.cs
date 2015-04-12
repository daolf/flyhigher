using UnityEngine;
using System.Collections;

public class SceneGeneratorScript : MonoBehaviour {

	public const int COGS_NB = 8;
	public int WIN;
	public int LOST; 
	public int WIN_SCORE; 
	public Score win_score;
	public Score myscore;

	public PrimaryCog[] cogs;
	public PrimaryCog cogToFind;
	public GameObject endMenu;
	public GameObject looseMenu;
	public SpriteRenderer gagnéBg;
	public SpriteRenderer perduBg;
	// position used to display an important cog...
	public Transform importantCogPosition;
	//Flag pour l'animation de fin 
	public bool isAnimEnd;
	public TimeBarscript timeBar;
	public bool gagné;
	public bool scoreNotUpdated;
	public bool check;
	public string scene;

	public Color normal;
	public Color win;
	public Color lost;
	public Color cogToFindColor;
	public Color goodCogColor;
	public bool hasPlayed;
	public GameObject cogsLevel1;
	public GameObject cogsLevel2;
	public GameObject cogsLevel3;
	private GameObject cogsLevel;
	// TODO put in prefab tuto
	public GameObject menupause;
	public GenericTutoScript tutoScript;
	public bool tuto;
	private int NbRealcogs;
	
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
		isPause = false;
		Time.timeScale = 1;
		initCogsLevel ();
		generateCogs ();
		scoreNotUpdated = true;

		win_score.value = WIN_SCORE;
		
		timeBar.endCallback = timerEndHandler;
	}

	// Use this for initialization
	void Start () {
		tuto = MecanoLevelConfiguration.tuto;
		updateSceneRoudFinish ();
		hasPlayed = false;
		if (tuto) {
			startFirstTuto();
		}
		isPause = tuto;
	}
	
	private void startFirstTuto() {
		menupause.SetActive(false);
		isPause = true;
		tutoScript.setBubbleVisibility(false);
		
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			tutoScript.say("Salut, je suis le mécanicien aéronautique, c'est grâce à moi que ton avion fonctionne ! Tu m'aides à retrouver la bonne pièce ?");
		};
		
		tutoScript.outCallback = delegate() {
			isPause = false;
			menupause.SetActive(true);
		};
		
		tutoScript.getIn();
	}

	private void initCogsLevel () {
		switch (MecanoLevelConfiguration.level) {
		case 1: 
			cogsLevel = cogsLevel1;
			break;
		case 2:
			cogsLevel = cogsLevel2;
			break;
		case 3:
			cogsLevel = cogsLevel2;
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

	void updateSceneRoudFinish () {
		destroySmothTranslation ();
		gagnéBg.enabled = false;
		perduBg.enabled = false;
		isAnimEnd = false;
		gagné = false;
		// replace cog
		generateCogs ();
		setAllSelectable ();
		// timer 
		timeBar.activated = true;
		hasPlayed = false;
	}

	void roundFinished() {
		if (isAnimEnd && gagné && !hasPlayed) {
			scoreNotUpdated = false;
			myscore.value += WIN;
			//TODO Menu or anim "great job !"
			updateSceneRoudFinish();
			scoreNotUpdated = true;
		} else if(isAnimEnd && !gagné && !hasPlayed) {
			scoreNotUpdated = false;
			if (myscore.value > LOST) {
				myscore.value -= LOST;
			} else {
				myscore.value = 0;
			}
			//TODO score in red ?
			updateSceneRoudFinish();
			scoreNotUpdated = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if (scoreNotUpdated) {
			roundFinished();
		}
		//if (Input.GetButtonDown ("Fire1") ){
		//	getoutOfTuto();
		//}
	}

	/*void getoutOfTuto() {
			tuto = false;
		if (role != null) {
			role.active = tuto;
			menupause.SetActive(true);
		}
			isPause = tuto;
			//updateSceneRoudFinish ();
	}*/

	private void timerEndHandler() {
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
		st.to = importantCogPosition.position;
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

	// called when a cog other than the cog to find is selected
	public void cogSelected(PrimaryCog cog) {
		if(cog.getCogId() == cogToFind.getCogId()) {
			setGoodCogFind(cog);
			cog.GetComponent<SpriteRenderer>().color = win;
			hasWon(true);
		} else {
			// may change later : for now, remove all other cogs than the selected
			// and the good one
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
			// TODO mettre isSelectable a false pour tout les cogs
			hasWon(false);
			//hasPlayed = true;
		}
	}

	//1 if victory, else 0
	public void hasWon(bool has) {
		gagné = has;
		if (has) {
			setAllUnselectable();
			timeBar.activated = false;
			gagnéBg.enabled = true;
		} else {
			setAllUnselectable();
			timeBar.activated = false;
			perduBg.enabled = true;

		}
	
	}

}
