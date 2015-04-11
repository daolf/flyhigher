using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject jeuPeintreButton;
	public GameObject levelPeintreButtons;

	public GameObject jeuMecanoButton;
	public GameObject levelMecanoButtons;

	public GameObject jeuPropulsionButton;
	public GameObject levelPropulsionButtons;

	public GameObject panel;

	// TO REMOVE later, initialize saved values...
	static bool isInitialized = false;

	void Start(){
		Time.timeScale = 1;
		
		// TO REMOVE later, initialize saved values...
		if(!isInitialized) {
			onEnable();
			isInitialized = true;
		}
		
		// only allow to choose unlocked levels
		int propulsionMaxLevel = PlayerPrefs.GetInt(Constants.PROPULSION_GAME_MAX_DIFFICULTY, 1);
		for(int i=propulsionMaxLevel; i<3; i++) {
			levelPropulsionButtons.transform.GetChild(i).gameObject.SetActive(false);
		}
	}
	//public SceneGenerator sg; // for Mecano
	
	void onEnable() {

		//Set the constant for the first time
		PlayerPrefs.SetInt (Constants.MAIN_GAME_HIGH_SCORE, 0);
		PlayerPrefs.SetInt (Constants.MAIN_GAME_MAX_LIFE, 0);
		PlayerPrefs.SetInt (Constants.PROPULSION_GAME_MAX_DIFFICULTY, 1);
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
		RectTransform rectPanel = panel.GetComponent<RectTransform>();
		LeanTween.moveX(jeuPeintreButton, 10000, 1.5f).setEase(LeanTweenType.easeInOutQuint);
		LeanTween.moveX(levelPeintreButtons, rectPanel.transform.position.x, 1.5f).setEase(LeanTweenType.easeInOutQuint);
	}

	public void Painterlevel1 () {
		Application.LoadLevel("PainterGameLvl0");
	}

	public void Painterlevel2 () {
		Application.LoadLevel("PainterGameLvl0");
	}

	public void Painterlevel3 () {
		Application.LoadLevel("PainterGameLvl0");
	}

	public void Propulsion () {
		RectTransform rectPanel = panel.GetComponent<RectTransform>();
		LeanTween.moveX(jeuPropulsionButton, 10000, 1.5f).setEase(LeanTweenType.easeInOutQuint);
		LeanTween.moveX(levelPropulsionButtons, rectPanel.transform.position.x, 1.5f).setEase(LeanTweenType.easeInOutQuint);
	}

	public void Propulsionlevel1 () {
		PropulsionLevelConfiguration.currentLevel = 1;
		Application.LoadLevel("IngameScene");
	}

	public void Propulsionlevel2 () {
		PropulsionLevelConfiguration.currentLevel = 2;
		Application.LoadLevel("IngameScene");
	}

	public void Propulsionlevel3 () {
		PropulsionLevelConfiguration.currentLevel = 3;
		Application.LoadLevel("IngameScene");
	}

	public void Mecano () {
		RectTransform rectPanel = panel.GetComponent<RectTransform>();
		LeanTween.moveX(jeuMecanoButton, 10000, 1.5f).setEase(LeanTweenType.easeInOutQuint);
		LeanTween.moveX(levelMecanoButtons, rectPanel.transform.position.x, 1.5f).setEase(LeanTweenType.easeInOutQuint);
	}

	public void Mecanolevel1 () {
		// TODO change accesibility to level when win
		MecanoLevelConfiguration.level = 1;
		MecanoLevelConfiguration.tuto = true;
		Application.LoadLevel("Mecano");
	}

	public void Mecanolevel2 () {
		MecanoLevelConfiguration.level = 2;
		MecanoLevelConfiguration.tuto = false;
		Application.LoadLevel("Mecano");
	}

	public void Mecanolevel3 () {
		MecanoLevelConfiguration.level = 3;
		MecanoLevelConfiguration.tuto = false;
		Application.LoadLevel("Mecano");
	}

}
