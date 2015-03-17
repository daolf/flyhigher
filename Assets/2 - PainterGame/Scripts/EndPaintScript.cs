using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndPaintScript : MonoBehaviour {

	public GameObject endMenu;
	public GameObject scoreText;
	public Text score;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		score.text = GetComponent<MainPaintScript> ().score.ToString();
		//On desactive l'affichage du score poru eviter les overlay avec le menu
		scoreText.GetComponent<Canvas> ().enabled = false;
		endMenu.GetComponent<Canvas>().enabled = true;
	}
}
