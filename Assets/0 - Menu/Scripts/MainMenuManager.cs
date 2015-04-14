using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
	
	// TO REMOVE later, initialize saved values...
	private static bool isInitialized = false;
	
	void Start() {
		//Set the constant for the first time
		PlayerPrefs.GetInt (Constants.MAIN_GAME_HIGH_SCORE, 0);
		PlayerPrefs.GetInt (Constants.MAIN_GAME_MAX_LIFE, 0);
		PlayerPrefs.GetInt (Constants.MAIN_GAME_ALREADY_PLAYED, 0);
		PlayerPrefs.GetInt (Constants.MAIN_GAME_LVL, 0);

		// Paint game
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL1_SUCCES, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL2_SUCCES, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL3_SUCCES, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL4_SUCCES, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL1_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL2_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL3_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_LVL4_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PAINT_GAME_ALREADY_PLAYED, 0);


		// Propulsion game
		PlayerPrefs.GetInt (Constants.PROPULSION_GAME_LVL1_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PROPULSION_GAME_LVL2_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PROPULSION_GAME_LVL3_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.PROPULSION_GAME_MAX_PLAYED, 0);
		PlayerPrefs.GetInt (Constants.PROPULSION_GAME_MAX_WON, 0);

		//Mecano game
		PlayerPrefs.GetInt (Constants.MECANO_GAME_LVL1_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.MECANO_GAME_LVL2_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.MECANO_GAME_LVL3_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.MECANO_GAME_LVL4_UNLOCK, 0);
		PlayerPrefs.GetInt (Constants.MECANO_GAME_ALREADY_PLAYED1, 0);
		PlayerPrefs.GetInt (Constants.MECANO_GAME_ALREADY_PLAYED2, 0);
		PlayerPrefs.GetInt (Constants.MECANO_GAME_ALREADY_PLAYED3, 0);


		
	}
	
	public void MainGame () {
		Application.LoadLevel("MainGame");
	}
	
	public void MiniGame () {
		Application.LoadLevel("MenuMiniGames");
	}
	
	

}