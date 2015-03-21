using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

	public Canvas menuPause;
	public GameObject game;

	public void OnPause() {

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
