using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	PauseScript pause;
	public GameObject game;
	public string scene;
	public Button bouttonPause;

	public void OnResume() {
		bouttonPause.image.enabled = true;
		bouttonPause.enabled = true;
		GetComponent<Canvas> ().enabled = false;
		Time.timeScale = 1;
		if (game.GetComponent<MainGame> ()) {
			game.GetComponent<MainGame> ().isPause = false;
		}
		if (game.GetComponent<ManagerPaint> ()) {
			game.GetComponent<ManagerPaint> ().isPause = false;
		}
		if (game.GetComponent<PipesGeneratorScript> ()) {
			game.GetComponent<PipesGeneratorScript> ().isPause = false;
		}
		if (game.GetComponent<SceneGeneratorScript> ()) {
			game.GetComponent<SceneGeneratorScript> ().isPause = false;
		}
		//pause.GetComponent<Sprite>().enabled = false;
	}


	public void OnRetry() {
		Time.timeScale = 1;
		Application.LoadLevel (scene);
	}

	public void OnQuit() {
		Time.timeScale = 1;
		if (Application.loadedLevelName == "MainGame") {
			Application.LoadLevel ("MainMenu");
		} else {
			Application.LoadLevel ("MenuMiniGames");
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
