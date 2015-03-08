using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeBarscript : MonoBehaviour {

	public RectTransform timeTransform;
	private float cachedY;
	private float minXValue;
	private float maxXValue;
	public float maxTime;
	private float currentTime;

	private int CurrentTime {
		get { return (int)currentTime;}
		set { currentTime = value;
			HandleTime();
		}
	}
	public Image visualtimebar;

	// Use this for initialization
	void Start () {
		cachedY = timeTransform.position.y;
		maxXValue = timeTransform.position.x;
		minXValue = timeTransform.position.x - timeTransform.rect.width;
		currentTime = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
		CurrentTime -= 1;
	}

	private void HandleTime () {
		float currentXValue = MapValues (currentTime, 0, maxTime, minXValue, maxXValue);
		timeTransform.position = new Vector3 (currentXValue, cachedY);
		if (currentTime > maxTime/2) { // from green to yellow
			visualtimebar.color = new Color32((byte)MapValues(currentTime,maxTime/2,maxTime,255,0),255,0,255);		
		} else { // from yellow to red
			visualtimebar.color = new Color32(255,(byte)MapValues(currentTime, 0, maxTime/2,0,255),0,255);
		}
	}


	private float MapValues (float x, float inMin, float inMax, float outMin, float outMax) {
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

}
