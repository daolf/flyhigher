using UnityEngine;
using System.Collections;

public class ManagerPaint : MonoBehaviour {
	
	public enum State {BEGIN,MAIN,ENDWIN,ENDLOOSE};
	public State state;
	public BeginPaintScript beginPaintScript ;
	public MainPaintScript mainPaintScript ;
	public Canvas endLooseMenu;
	public Canvas endWinMenu;
	public bool isPause;
	public GenericTutoScript tutoScript;
	// Use this for initialization
	void Start () {

		state = State.BEGIN;
		isPause = false;
		beginPaintScript.enabled = true;
		mainPaintScript.enabled = false;
		endLooseMenu.enabled = false;
		endWinMenu.enabled = false;
		Time.timeScale = 1;
		firstPlayTuto();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		switch (state) {
		case State.BEGIN :
			break;

		case State.MAIN :
			beginPaintScript.enabled = false;
			mainPaintScript.enabled = true;
			endLooseMenu.enabled = false;
			endWinMenu.enabled = false;
			break;

		case State.ENDLOOSE :
			beginPaintScript.enabled = false;
			mainPaintScript.enabled = false;
			endLooseMenu.enabled = true;
			endWinMenu.enabled = false;
			break;

		case State.ENDWIN :
			beginPaintScript.enabled = false;
			mainPaintScript.enabled = false;
			endLooseMenu.enabled = false;
			endWinMenu.enabled = true;
			break;
		}
	}

	/**
	 * First tutorial
	 */
	private void firstPlayTuto() {
		tutoScript.setBubbleVisibility(false);
		
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			//si c'est la première fois présentation , 
			if(PlayerPrefs.GetInt (Constants.PAINT_GAME_ALREADY_PLAYED) == 0 ) {
			tutoScript.say("Bonjour, je m'appelle Victor et je suis peintre aéronautique.. Mon métier consiste à peindre toutes les parties de l'avion. Attention, ce métier n'est pas aussi simple qu'il n'y parait.");
			}
			else {
				float r = Random.value;
				print (r);
				if (r < 0.5) {
					tutoScript.say("Cas 1");
				}
				else {
					tutoScript.say("Cas 2");
				}
			}
			PlayerPrefs.SetInt(Constants.PAINT_GAME_ALREADY_PLAYED,1);

		};
		
		tutoScript.outCallback = delegate() {
			state = State.MAIN;
		};
		
		tutoScript.getIn();
	}
}
