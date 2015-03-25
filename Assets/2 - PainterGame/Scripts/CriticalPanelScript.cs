using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CriticalPanelScript : MonoBehaviour {

	public enum CriticalState { CRITICAL, NORMAL};
	public CriticalState criticalState;

	private enum UpDownState { UP, DOWN};
	private UpDownState upDownState;

	private float limAlpha = 0.2f;
	// Use this for initialization
	void Start () {
		criticalState = CriticalState.NORMAL;
		upDownState = UpDownState.UP;
	}
	
	// Update is called once per frame
	void OnGUI () {
		Color tmpColor = transform.GetComponent<SpriteRenderer>().color;
		if(criticalState == CriticalState.CRITICAL)
		{
			if(upDownState == UpDownState.UP && tmpColor.a < limAlpha)
			{
				tmpColor.a += 0.01f;

			}
			else if(upDownState == UpDownState.DOWN && tmpColor.a > 0)
			{
				tmpColor.a -= 0.01f;
			}
			else if(upDownState == UpDownState.DOWN)
				upDownState = UpDownState.UP;
			else if(upDownState == UpDownState.UP)
				upDownState = UpDownState.DOWN;

			transform.GetComponent<SpriteRenderer>().color = tmpColor;
		}
		else {
			tmpColor.a = 0;
			transform.GetComponent<SpriteRenderer>().color = tmpColor;
		}
	}
}
