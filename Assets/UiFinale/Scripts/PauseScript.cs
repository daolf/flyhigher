using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour {

	public Canvas menuPause;
	public GameObject game;
	public Button buttonPause;

	public void OnPause() {
		buttonPause.image.enabled = false;
		buttonPause.enabled = false;
		Time.timeScale = 0;
		menuPause.enabled = true;
		game.GetComponent<MainGame> ().isPause = true;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
