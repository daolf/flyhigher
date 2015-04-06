using UnityEngine;
using System.Collections;

public class textBeforeGame : MonoBehaviour {

	public SceneGeneratorScript game;

	// Use this for initialization
	void Start () {
		game.GetComponent<SceneGeneratorScript> ().isPause = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
