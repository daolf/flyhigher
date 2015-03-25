using UnityEngine;
using System.Collections;

public class RotatableItemScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
			gameObject.transform.Rotate (0, 0, 90);
	}
}
