using UnityEngine;
using System.Collections;

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

	void Update () {
	
	}
}

