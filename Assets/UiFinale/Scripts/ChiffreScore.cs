using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChiffreScore : MonoBehaviour {

	public string resourceName = "NombreScore";
	public Sprite[] myNumbers;
	public int value;

	// Use this for initialization
	void Start () {
		myNumbers = Resources.LoadAll<Sprite>(resourceName);
	}

	public void OnGUI() {
			GetComponent<Image> ().sprite = myNumbers [value];
	}

	// Update is called once per frame
	void Update () {

	}
}
