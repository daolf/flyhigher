using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Objectif : MonoBehaviour {

	private int objectif;

	public void setObjectif(int obj){
		this.GetComponentInChildren<Text> ().text = "Objectif : " + obj;
	}

	public void getInAndOut() {
		float x = gameObject.GetComponent<RectTransform> ().position.x;
		float y = gameObject.GetComponent<RectTransform> ().position.y;
		float height = gameObject.GetComponent<RectTransform> ().sizeDelta.x;

		LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(300,310), 1.5f)
			.setEase(LeanTweenType.easeOutQuint);

		StartCoroutine (waitCoroutine ());
	}



	private IEnumerator waitCoroutine() {
		yield return new WaitForSeconds(1.5f);
		LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(300,386), 1.5f)
			.setEase(LeanTweenType.easeInQuint);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
