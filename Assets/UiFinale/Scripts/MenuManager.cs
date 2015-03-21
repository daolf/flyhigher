using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	PauseScript pause;
	public GameObject game;
	public string scene;

	public void OnResume() {
		GetComponent<Canvas> ().enabled = false;
		Time.timeScale = 1;
		game.GetComponent<MainGame> ().isPause = false;
		//pause.GetComponent<Sprite>().enabled = false;
	}


	public void OnRetry() {
		Time.timeScale = 1;
		Application.LoadLevel (scene);
	}

	public void OnQuit() {
		Time.timeScale = 1;
		Application.LoadLevel ("MainMenu");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
