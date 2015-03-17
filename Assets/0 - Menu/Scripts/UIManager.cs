using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {


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
		Application.LoadLevel("main");
	}

}
