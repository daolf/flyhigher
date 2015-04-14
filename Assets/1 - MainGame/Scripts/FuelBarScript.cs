using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FuelBarScript : MonoBehaviour {

	public GameObject bar;
	private RectTransform rectTransform;
	public float progress;
	// Use this for initialization
	void Start () {
		rectTransform = bar.GetComponent<RectTransform>();
	}

	private void OnGUI() {
		rectTransform.localScale = new Vector2(progress, 1);
	}

	public void hide(){
		foreach(Image tr in this.GetComponentsInChildren<Image>()){
			tr.enabled = false;
		}
	}

	public void show(){
		foreach(Image tr in this.GetComponentsInChildren<Image>()){
			tr.enabled = true;
		}
	}

	void Update () {
	
	}
}

