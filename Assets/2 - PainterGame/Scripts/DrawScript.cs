using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawScript : MonoBehaviour {

	public Transform tachePrefab;
	public Transform paintPrefab;
	public Text textScore;
	public int score;
	public int gain ;
	public int perte ;
	public Transform buffer;

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
	}	

	// Update is called once per frame
	void Update () {
		Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if(Input.GetMouseButton(0) && 
		   GetComponent<Collider2D>() == Physics2D.OverlapPoint(new Vector2(pz.x,pz.y))
		   ){
			drawPaint(pz);
			updateScore(gain);
			print (score);
		}		
	}
}
