﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainPaintScript : MonoBehaviour {

	public Transform tachePrefab;
	public Transform paintPrefab;
	public Text textScore;
	public int score;
	public int gain ;
	public int perte ;
	public Transform buffer;
	public float speed;
	public float movement;
	public float previousX;
	public float maxMovement;
	public bool endScroll = false;
	
	public void drawPaint(Vector3 pz) {
		buffer = Instantiate(paintPrefab,new Vector3(pz.x,pz.y,-1), Quaternion.identity) as Transform;	
		if (buffer == null) {
			print("erreur buffer null");		
		}
		buffer.transform.parent = GameObject.Find("background").transform;
	}
	
	public void drawTache(Vector3 pz) {
		buffer = Instantiate(tachePrefab,new Vector3(pz.x,pz.y,-1), Quaternion.identity) as Transform;	
		if (buffer == null) {
			print("erreur buffer null");		
		}
		buffer.transform.parent = GameObject.Find("background").transform;
		
	}
	
	public void updateScore(int scoreTemp) {
		score = score + scoreTemp;
		textScore.text = "Score :" + score;
	}
	
	void OnMouseExit() {
		Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);	
		if(Input.GetMouseButton(0)){
			drawTache(pz);
			print("perte" + this.perte);
			updateScore(this.perte);
		}
	}
	
	
	// Use this for initialization
	void Start () {		
		speed = 4;
		movement = 0;
		previousX = this.transform.position.x;
		// TODO modify calculation of maxMovement
		maxMovement = this.GetComponent<Renderer>().bounds.size.x - (float)10.5 ;
		score = 0;
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

			//Application.LoadLevel("MenuEndPaint");

			//On passe le manager dans l'état fin
			GetComponent<ManagerPaint>().state = ManagerPaint.State.END;
		}
	}

	
	// Update is called once per frame
	void Update () {
		Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		if(Input.GetMouseButton(0) && 
		   GetComponent<Collider2D>() == Physics2D.OverlapPoint(new Vector2(pz.x,pz.y))
		   ){
			drawPaint(pz);
			print ("la");
			updateScore(gain);
			print (score);
		}		
	
	
	
	}
}