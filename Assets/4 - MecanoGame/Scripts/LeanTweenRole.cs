using UnityEngine;
using System.Collections;

public class LeanTweenRole : MonoBehaviour {

	public GameObject role;
	// Use this for initialization
	void Start () {
		LeanTween.moveX(role, 257, 1.5f).setEase(LeanTweenType.easeInOutQuint);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
