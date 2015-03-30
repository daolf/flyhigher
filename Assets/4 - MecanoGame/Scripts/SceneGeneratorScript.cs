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

	// Use this for initialization
	void Start () {
		isAnimEnd = false;
		Time.timeScale = 1;
		cogToFind.setCogId(Random.Range(0, cogs.Length));
		gagnéBg.enabled = false;
		perduBg.enabled = false;
		
		timeBar.endCallback = timerEndHandler;

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
	
	// Update is called once per frame
	void Update () {
		if(isAnimEnd && gagné) {
			endMenu.GetComponent<Canvas>().enabled = true;
		}
		else if(isAnimEnd && !gagné) {
			looseMenu.GetComponent<Canvas>().enabled = true;
		}
	}
	
	private void timerEndHandler() {
		setAllUnselectable();
		looseMenu.GetComponent<Canvas> ().enabled = true;
	}

	private void setGoodCogFind(PrimaryCog goodOne) {
		goodOne.setSelectable(false);
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

	// called when a cog other than the cog to find is selected
	public void cogSelected(PrimaryCog cog) {
		if(cog.getCogId() == cogToFind.getCogId()) {
			setGoodCogFind(cog);
			Destroy (cog);
			hasWon(true);
		}
		else {
			// may change later : for now, remove all other cogs than the selected
			// and the good one
			PrimaryCog goodOne = null;

			for (int i=0; i<COGS_NB; i++) {
				if(cogs[i] != cog && cogs[i].getCogId() != cogToFind.getCogId()) {
					//cogs[i].enabled = false;
					//Destroy(cogs[i].gameObject);
				}
				else if(cogs[i].getCogId() == cogToFind.getCogId()) {
					goodOne = cogs[i];
				}
			}

			if(goodOne) {
				setGoodCogFind(goodOne);
			}
				Destroy (cog);
			// TODO mettre isSelectable a false pour tout les cogs
			hasWon(false);
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
