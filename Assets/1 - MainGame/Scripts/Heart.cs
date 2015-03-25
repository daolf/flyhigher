using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Heart : MonoBehaviour {

	public bool full;
	public Sprite heartFull;
	public Sprite heartEmpty;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (full) {
			this.GetComponent<Image> ().sprite = heartFull;
		} else {
			this.GetComponent<Image> ().sprite = heartEmpty;
		}
	}
}
