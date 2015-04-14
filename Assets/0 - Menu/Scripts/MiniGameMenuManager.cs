using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
		int propulsionMaxLevel = PlayerPrefs.GetInt (Constants.PROPULSION_GAME_MAX_DIFFICULTY, 1);
		for (int i=propulsionMaxLevel; i<3; i++) {
			levelPropulsionButtons.transform.GetChild (i).GetComponent<Button>().interactable = false;
		}


		//Make button disabled if level not unlocked
		if (PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL1_UNLOCK) == 0) {
			levelPeintreButtons.transform.FindChild ("LevelPeintre1").GetComponent<Button> ().interactable = false;
		}
		if (PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL2_UNLOCK) == 0) {
			levelPeintreButtons.transform.FindChild ("LevelPeintre2").GetComponent<Button> ().interactable = false;
		}
		if (PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL3_UNLOCK) == 0) {
			levelPeintreButtons.transform.FindChild ("LevelPeintre3").GetComponent<Button> ().interactable = false;
		}
		if (PlayerPrefs.GetInt (Constants.MECANO_GAME_LVL1_UNLOCK) == 0) {
			levelMecanoButtons.transform.FindChild ("LevelMecano1").GetComponent<Button> ().interactable = false;
		}
		if (PlayerPrefs.GetInt (Constants.MECANO_GAME_LVL2_UNLOCK) == 0) {
			levelMecanoButtons.transform.FindChild ("LevelMecano2").GetComponent<Button> ().interactable = false;
		}
		if (PlayerPrefs.GetInt (Constants.MECANO_GAME_LVL3_UNLOCK) == 0) {
			levelMecanoButtons.transform.FindChild ("LevelMecano3").GetComponent<Button> ().interactable = false;
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
		Application.LoadLevel("PainterGameLvl1");
	}

	public void Painterlevel3 () {
		Application.LoadLevel("PainterGameLvl2");
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
