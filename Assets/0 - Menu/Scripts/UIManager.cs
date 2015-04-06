using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject jeuPeintreButton;
	public GameObject levelPeintreButton;

	public GameObject jeuMecanoButton;
	public GameObject levelMecanoButton;

	public GameObject jeuPropulsionButton;
	public GameObject levelProplusionButton;

	//public GameObject jeuMecanoButton;

	void onEnable() {
		//Set the constant for the first time
		PlayerPrefs.GetInt (Constants.MAIN_GAME_HIGH_SCORE, 0);
		PlayerPrefs.GetInt (Constants.MAIN_GAME_MAX_LIFE, 0);
	}

	public void Menu() {
		Application.LoadLevel("MainMenu");
	}

	public void MainGame () {
		Application.LoadLevel("MainGame");
	}

	public void MiniGame () {
		Application.LoadLevel("MenuMiniGames");
	}
	
	public void Painter () {
		//Application.LoadLevel("PainterGameLvl0");
		LeanTween.moveX(jeuPeintreButton, 10000, 1.5f).setEase(LeanTweenType.easeInOutQuint);
		LeanTween.moveX(levelPeintreButton, 337, 1.5f).setEase(LeanTweenType.easeInOutQuint);
	}

	public void Painterlevel1 () {
	
	}

	public void Painterlevel2 () {
		
	}

	public void Painterlevel3 () {
		
	}

	public void Propulsion () {
		//Application.LoadLevel("IngameScene");
		LeanTween.moveX(jeuPropulsionButton, 10000, 1.5f).setEase(LeanTweenType.easeInOutQuint);
		LeanTween.moveX(levelProplusionButton, 337, 1.5f).setEase(LeanTweenType.easeInOutQuint);
	}

	public void Propulsionlevel1 () {
		
	}

	public void Propulsionlevel2 () {
		
	}

	public void Propulsionlevel3 () {
		
	}

	public void Mecano () {
		//Application.LoadLevel("main");
		LeanTween.moveX(jeuMecanoButton, 10000, 1.5f).setEase(LeanTweenType.easeInOutQuint);
		LeanTween.moveX(levelMecanoButton, 337, 1.5f).setEase(LeanTweenType.easeInOutQuint);

	}

	public void Mecanolevel1 () {
		
	}

	public void Mecanolevel2 () {
		
	}

	public void Mecanolevel3 () {
		
	}

}
