using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
	
	// TO REMOVE later, initialize saved values...
	private static bool isInitialized = false;

	void Start() {		
		//Set the constant for the first time
		PlayerPrefs.GetInt (Constants.MAIN_GAME_HIGH_SCORE, 0);
		PlayerPrefs.GetInt (Constants.MAIN_GAME_MAX_LIFE, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL1_UNLOCK, 1);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL2_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL3_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL4_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_ALREADY_PLAYED, 0);
		
		// TO REMOVE later, initialize saved values...
		if(!isInitialized) {
			initializeSave();
			isInitialized = true;
		}
		
	}
	
	public void MainGame () {
		Application.LoadLevel("MainGame");
	}
	
	public void MiniGame () {
		Application.LoadLevel("MenuMiniGames");
	}
	
	
	void initializeSave() {
		
		//Set the constant for the first time
		PlayerPrefs.SetInt (Constants.MAIN_GAME_HIGH_SCORE, 0);
		PlayerPrefs.SetInt (Constants.MAIN_GAME_MAX_LIFE, 0);
		PlayerPrefs.SetInt (Constants.PROPULSION_GAME_MAX_DIFFICULTY, 1);
	}
}
