using UnityEngine;
using System.Collections;

public class MiniGameMenuManager : MonoBehaviour {

	public GameObject jeuPeintreButton;
	public GameObject levelPeintreButtons;

	public GameObject jeuMecanoButton;
	public GameObject levelMecanoButtons;

	public GameObject jeuPropulsionButton;
	public GameObject levelPropulsionButtons;

	public RectTransform rectPanel;

	void Start(){
		Time.timeScale = 1;
		
		// only allow to choose unlocked levels
		// FIXME should probably mark locked level instead of de-activating them
		int propulsionMaxLevel = PlayerPrefs.GetInt(Constants.PROPULSION_GAME_MAX_DIFFICULTY, 1);
		for(int i=propulsionMaxLevel; i<3; i++) {
			levelPropulsionButtons.transform.GetChild(i).gameObject.SetActive(false);
		}
	}
	//public SceneGenerator sg; // for Mecano


	public void Menu() {
		Application.LoadLevel("MainMenu");
	}
	
	public void Painter () {
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
		MecanoLevelConfiguration.tuto = true;
		Application.LoadLevel("Mecano");
	}

	public void Mecanolevel3 () {
		MecanoLevelConfiguration.level = 3;
		MecanoLevelConfiguration.tuto = true;
		Application.LoadLevel("Mecano");
	}

}
