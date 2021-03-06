﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeBarscript : MonoBehaviour {

	public RectTransform timeTransform;
	public Sprite gameHover;
	//private float cachedY;
	private float minXValue;
	private float maxXValue;
	public float maxTime;
	private float currentTime;
	public Gradient g;
	public GradientColorKey[] gck;
	public GradientAlphaKey[] gak;
	public bool activated = true;

	public delegate void TimerEndCallback();
	public TimerEndCallback endCallback = null;


	public float CurrentTime {
		get { return currentTime;}
		set { currentTime = value;
			HandleTime();
		}
	}
	public Image visualtimebar;

	// Use this for initialization
	void Start () {
		//activated = true;
		//cachedY = timeTransform.position.y;
		
		minXValue = 0;
		maxXValue = timeTransform.rect.width;
		
		//currentTime = maxTime;
		g = new Gradient();

		// Populate the color keys at the relative time 0 and 1 (0 and 100%)
		gck = new GradientColorKey[3];
		gck[0].color = new Color32(9,183,62,1);
		gck[0].time = 0.0f;
		gck[1].color = new Color32(255,228,0,1);
		gck[1].time = 0.5f;
		gck[2].color = new Color32(219,101,63,1);
		gck[2].time = 1.0f;
		
		// Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
		gak = new GradientAlphaKey[2];
		gak[0].alpha = 1.0f;
		gak[0].time = 0.0f;
		gak[1].alpha = 1.0f;
		gak[1].time = 1.0f;
		g.SetKeys(gck, gak);

		CurrentTime = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			if (CurrentTime > 0) {
				CurrentTime -= Time.deltaTime;
			} else {
				endCallback();
				activated = false;
			}
		}
		
	}

	private void HandleTime () {
		float currentXValue = MapValues (currentTime, 0, maxTime, maxXValue, minXValue);
		timeTransform.offsetMax = new Vector2(-currentXValue , timeTransform.offsetMax.y);
		visualtimebar.color = g.Evaluate ((maxTime -currentTime)/maxTime);
	}

	private float MapValues (float x, float inMin, float inMax, float outMin, float outMax) {
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

}
