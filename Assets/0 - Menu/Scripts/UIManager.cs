using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
	
	// Update is called once per frame
	public void MainGame () {
		Application.LoadLevel("MainGame");
	}

	// Use this for initialization
	public void MiniGame () {
		Application.LoadLevel("MenuMiniGames");
	}
	
	// Update is called once per frame
	public void Painter () {
		Application.LoadLevel("PainterGameLvl0");
	}

	// Update is called once per frame
	public void Propulsion () {
		Application.LoadLevel("IngameScene");
	}
	
	// Update is called once per frame
	public void Mecano () {
		Application.LoadLevel("main");
	}

}
