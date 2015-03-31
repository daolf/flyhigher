using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject buttonMecano;

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
		Application.LoadLevel("PainterGameLvl0");
	}

	public void Propulsion () {
		Application.LoadLevel("IngameScene");
	}
	
	public void Mecano () {
		//Application.LoadLevel("main");
		LeanTween.moveX(buttonMecano, 10000, 1.5f).setEase(LeanTweenType.easeInOutQuint);
	}

}
