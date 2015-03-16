using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndPaintScript : MonoBehaviour {

	public GameObject endMenu;
	public Text score;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		score.text = GetComponent<MainPaintScript> ().score.ToString();
		endMenu.GetComponent<Canvas>().enabled = true;
	}
}
