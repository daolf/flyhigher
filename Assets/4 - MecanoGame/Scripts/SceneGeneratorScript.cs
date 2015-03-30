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

	public bool hasPlayed;
	public GameObject cogsLevel;
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

	// Use this for initialization
	void Start () {
		isPause = false;
		Time.timeScale = 1;
		initCogsLevel ();
		generateCogs ();
		scoreNotUpdated = true;
		updateSceneRoudFinish ();
		hasPlayed = false;
		win_score.value = WIN_SCORE;
		
		timeBar.endCallback = timerEndHandler;
	}

	private void initCogsLevel () {
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
		// initialize all cogs with "random" ids (in fact each one need to be uniq, so its a shuffle)
		int[] cogIds = new int[cogs.Length];
		for(int i=0; i<cogs.Length; i++)
			cogIds[i] = i;
		// swap 20 times
		for(int step=0; step<20; step++) {
			int a = Random.Range(0, cogs.Length);
			int b = Random.Range(0, cogs.Length);
			int swap = cogIds[a];
			cogIds[a] = cogIds[b];
			cogIds[b] = swap;
		}
		
		// set cog ids and speed
		for(int i=0; i<cogs.Length; i++) {
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
	}
	
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
			}
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
