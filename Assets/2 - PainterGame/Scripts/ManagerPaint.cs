using UnityEngine;
using System.Collections;

public class ManagerPaint : MonoBehaviour {
	
	public enum State {BEGIN,MAIN,END};
	public State state;
	public BeginPaintScript beginPaintScript ;
	public MainPaintScript mainPaintScript ;
	public EndPaintScript endPaintScript ;
	public bool isPause;
	// Use this for initialization
	void Start () {

		state = State.BEGIN;
		isPause = false;
		beginPaintScript.enabled = true;
		mainPaintScript.enabled = false;
		endPaintScript.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		switch (state) {
		case State.BEGIN :
			state = State.MAIN;
			break;

		case State.MAIN :
			beginPaintScript.enabled = false;
			mainPaintScript.enabled = true;
			endPaintScript.enabled = false;
			break;

		case State.END :
			beginPaintScript.enabled = false;
			mainPaintScript.enabled = false;
			endPaintScript.enabled = true;
			break;
		}
	}
}
