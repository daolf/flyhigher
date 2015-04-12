using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TutoHandScript : MonoBehaviour {

	public Sprite handNormal;
	public Sprite handClick;
	
	public enum HandKind { HandNormal, HandClick };
	
	
	private Image thisImage;

	// Use this for initialization
	void Start () {
		thisImage = GetComponent<Image>();
	}
	
	
	public void setVisibility(bool visibility) {
		gameObject.SetActive(visibility);
	}
	
	public void setHandKind(HandKind kind) {
		switch(kind) {
			case HandKind.HandNormal:
				thisImage.sprite = handNormal;
				break;
			case HandKind.HandClick:
				thisImage.sprite = handClick;
				break;
		}
	}
	
	/**
	 * Move (using LeanTween InOutSine tween) the hand to a given position in world coordinates.
	 * Use duration = 0 to instant move the hand...
	 * FIXME check is LeanTween works when not active!
	 */
	public void moveToWorldPosition(Vector3 destPos, float duration) {
		// really, I don't understand why it works, but it works...
		Vector3 screenPos = Camera.main.WorldToScreenPoint(destPos);
		LeanTween.move(gameObject, screenPos, duration).setEase(LeanTweenType.easeInOutSine);
	}
}
