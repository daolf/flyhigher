using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
	
	// TO REMOVE later, initialize saved values...
	private bool isInitialized = false;

	void Start() {
		
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
