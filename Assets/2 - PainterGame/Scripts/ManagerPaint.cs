using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManagerPaint : MonoBehaviour {
	
	public enum State {BEGIN,MAIN,ENDWIN,ENDLOOSE};
	public State state;
	public BeginPaintScript beginPaintScript ;
	public MainPaintScript mainPaintScript ;
	public Canvas endLooseMenu;
	public Canvas endWinMenu;
	public bool isPause;
	public PausePaintScript pauseButton;
	public GenericTutoScript tutoScript;
	public TutoHandScript hand;
	// Use this for initialization
	void Start () {

		state = State.BEGIN;
		isPause = false;
		beginPaintScript.enabled = true;
		mainPaintScript.enabled = false;
		endLooseMenu.enabled = false;
		endWinMenu.enabled = false;
		Time.timeScale = 1;
		if (PlayerPrefs.GetInt (Constants.PAINT_GAME_ALREADY_PLAYED) == 0) {
			tutoFirstPlayed ();
		} else {
			tuto ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		switch (state) {
		case State.BEGIN :
			pauseButton.GetComponent<Button>().interactable = false;
			break;

		case State.MAIN :
			pauseButton.GetComponent<Button>().interactable = true;
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
	 * Tuto
	 */
	private void tutoFirstPlayed() {

		tutoScript.setBubbleVisibility(false);
		tutoScript.readyCallback = delegate() {
			tutoScript.setBubbleVisibility(true);
			
			string[] messages = new string[] {
				"Le but du jeu, c'est de peindre la carlingue de l'avion.",
				"Pour cela, ton doigt doit rester sur le trait à peindre. L'objectif est d'avoir un taux de réussite de 100%.",
				" Attention, si tu dépassesl'écran, ton taux de réussite diminuera !"
			};
			tutoScript.say(messages);
		};

		tutoScript.dialogueEndCallback = delegate() {
			tutoScript.dialogueEndCallback = null;
			tutoScript.setBubbleVisibility(false);
			tutoScript.hand.moveToWorldPosition(new Vector3(0,0,0), 1.8f);
			StartCoroutine(testCoroutine());
		};
		
		tutoScript.outCallback = delegate() {
			state = State.MAIN;
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

	/**
	 * Information
	 */
	private void tuto() {
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
