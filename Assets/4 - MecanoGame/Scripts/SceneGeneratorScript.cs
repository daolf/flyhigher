using UnityEngine;
using System.Collections;

public class SceneGeneratorScript : MonoBehaviour {

	public const int COGS_NB = 8;

	public PrimaryCog[] cogs = new PrimaryCog[COGS_NB];
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
	public Score myscore;
	public bool hasPlayed;
	public bool isPause;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		generateCogs ();
		scoreNotUpdated = true;
		updateSceneRoudFinish ();
		hasPlayed = false;
		isPause = false;
	}

	public void generateCogs () {
		cogToFind.setCogId(Random.Range(0, cogs.Length));
		// initialize all cogs with "random" ids (in fact each one need to be uniq, so its a shuffle)
		int[] cogIds = new int[COGS_NB];
		for(int i=0; i<COGS_NB; i++)
			cogIds[i] = i;
		// swap 20 times
		for(int step=0; step<20; step++) {
			int a = Random.Range(0, COGS_NB);
			int b = Random.Range(0, COGS_NB);
			int swap = cogIds[a];
			cogIds[a] = cogIds[b];
			cogIds[b] = swap;
		}
		
		// set cog ids and speed
		for(int i=0; i<COGS_NB; i++) {
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
			myscore.value += 10;
			//TODO Menu or anim "great job !"
			updateSceneRoudFinish();
			scoreNotUpdated = true;
		} else if(isAnimEnd && !gagné && !hasPlayed) {
			scoreNotUpdated = false;
			if (myscore.value > 5) {
				myscore.value -= 5;
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
		for(int i=0; i<COGS_NB; i++) {
			cogs[i].setSelectable(false);
		}

	}

	public void destroySmothTranslation () {
		Destroy(cogToFind.gameObject.GetComponent("SmoothTranslation"));
		cogToFind.setSelectable (false);
		for(int i=0; i<COGS_NB; i++) {
			Destroy(cogs[i].gameObject.GetComponent("SmoothTranslation"));
		}
	}

	public void setAllSelectable () {
		cogToFind.setSelectable (false);
		for(int i=0; i<COGS_NB; i++) {
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

			for (int i=0; i<COGS_NB; i++) {
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
