using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveBackground : MonoBehaviour {
	
	// Ici on gère le déplacement du BackGround

	public float speed;
	public float movement;
	public float previousX;
	public float maxMovement;
	public Text textScore;
	public bool endScroll = false;
	public GameObject endMenu;
	public GameObject DrawScript;
	//End of scroll pop up
	void EndScrollWindow(int windowID) {
		new Rect(10, 20, 100, 20);
	}



	// Use this for initialization
	void Start () {

		speed = 4;
		movement = 0;
		previousX = this.transform.position.x;
		// TODO modify calculation of maxMovement
		maxMovement = this.GetComponent<Renderer>().bounds.size.x - (float)10.5 ;
	
		endMenu.SetActive(false);
	}


	void FixedUpdate() {

		// We check we didn't get out off the screen
		// Check if not out of the screen
		if (movement < maxMovement) {
						previousX = this.transform.position.x;
						this.transform.Translate (Vector3.up * speed * Time.deltaTime);
						movement += (this.transform.position.x - previousX);
				}
		// end of the game we change the scene
		else {
			//TODO tell if win or loose
			endScroll = true;
			textScore.text = GetComponent<DrawScript>().score.ToString();
			GetComponent<DrawScript>().enabled = false ;
			endMenu.SetActive(true);
			//Application.LoadLevel("MenuEndPaint");
				}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
