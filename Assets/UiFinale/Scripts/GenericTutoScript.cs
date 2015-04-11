using UnityEngine;
using System.Collections;

public class GenericTutoScript : MonoBehaviour {

	public AutoType textField;

	// Use this for initialization
	void Start () {
		LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(258, 152), 1.5f)
			.setEase(LeanTweenType.easeInOutQuint);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
