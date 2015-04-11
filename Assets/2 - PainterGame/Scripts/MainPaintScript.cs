using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainPaintScript : MonoBehaviour {

	public Transform tachePrefab;
	public Transform paintPrefab;
	public Transform patternPrefab;
	public Pourcent guiScore;
	public float objectif;
	public float score;
	public int gain ;
	public int perte ;
	public Transform buffer;
	public float speed;
	public float movement;
	public float previousX;
	public float maxMovement;
	public bool endScroll = false;
	public bool onCanvas ;
	public GameObject endMenu;
	public CriticalPanelScript criticalPanel;

	public void drawPaint(Vector3 pz) {
		buffer = Instantiate(paintPrefab,new Vector3(pz.x,pz.y,this.GetComponent<Transform>().position.z), Quaternion.identity) as Transform;	
		if (buffer == null) {
			print("erreur buffer null");		
		}
		buffer.transform.parent = GameObject.Find("background").transform;
	}
	
	public void drawTache(Vector3 pz) {
		buffer = Instantiate(tachePrefab,new Vector3(pz.x,pz.y,this.GetComponent<Transform>().position.z), Quaternion.identity) as Transform;	
		if (buffer == null) {
			print("erreur buffer null");		
		}
		buffer.transform.parent = GameObject.Find("background").transform;
		
	}
	
	public void updateScore(int scoreTemp) {
		score = score + scoreTemp;
		if (score < 0) {
			score = 0;
		}
		float pct = (score / objectif) * 100;
		guiScore.value = (int) pct;
	}

	
	
	// Use this for initialization
	void Start () {		
		speed = (float)3;
		movement = 0;
		previousX = this.transform.position.x;
		// TODO modify calculation of maxMovement
		maxMovement = this.GetComponent<Renderer> ().bounds.size.x;
		score = 0;
		endMenu.GetComponent<Canvas> ().enabled = false;
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

			endMenu.GetComponent<Canvas>().enabled = true;

			//On passe le manager dans l'état fin
			if(score < objectif ) {
				GetComponent<ManagerPaint>().state = ManagerPaint.State.ENDLOOSE;
			}
			else {
				GetComponent<ManagerPaint>().state = ManagerPaint.State.ENDWIN;
			}
		}
	}

	
	// Update is called once per frame
	void Update () {
		Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		foreach (Touch touch in Input.touches)
		{
			int pointerID = touch.fingerId;
			if (EventSystem.current.IsPointerOverGameObject(pointerID))
			{
				// at least on touch is over a canvas UI
				onCanvas = true;
				return;
			}
			
			else 
			{
				// here we don't know if the touch was over an canvas UI
				onCanvas = false;
			}
		}
		if (!GetComponentInParent<ManagerPaint> ().isPause &&
		     !onCanvas){

			if (Input.GetMouseButton (0) && 
				GetComponent<Collider2D> () == Physics2D.OverlapPoint (new Vector2 (pz.x, pz.y))
		   ) {
				drawPaint (pz);
				updateScore (gain);
				criticalPanel.criticalState = CriticalPanelScript.CriticalState.NORMAL;
			} else if (Input.GetMouseButton (0) && 
				GetComponent<Collider2D> () != Physics2D.OverlapPoint (new Vector2 (pz.x, pz.y)) ) {
				drawTache (pz);
				criticalPanel.criticalState = CriticalPanelScript.CriticalState.CRITICAL;
				updateScore (perte);
			}
			else {
				criticalPanel.criticalState = CriticalPanelScript.CriticalState.NORMAL;
			}
	
		}
	
	}
}
