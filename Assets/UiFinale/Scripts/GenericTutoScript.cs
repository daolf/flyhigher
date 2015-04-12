﻿using UnityEngine;
using System.Collections;

public class GenericTutoScript : MonoBehaviour {

	public AutoType textField;
	
	public delegate void StateChangeDelegate();
	public StateChangeDelegate dialogueEndCallback = null;
	public StateChangeDelegate readyCallback = null;
	public StateChangeDelegate outCallback = null;
	
	private enum TutoState { Hidden, Ready, Talking, Waiting };
	TutoState state = TutoState.Hidden;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( (state == TutoState.Talking || state == TutoState.Waiting)
		   && Input.GetButtonDown("Fire1"))
		{
			if(state == TutoState.Talking && !textField.isComplete()) {
				// instant display of the rest of the message
				textField.instantDisplay();
				state = TutoState.Waiting;
			}
			else {
				if(dialogueEndCallback != null)
					dialogueEndCallback();
				else
					getOut();
			}
				
		}
	}
	
	public void setBubbleVisibility(bool visibility) {
		transform.GetChild(0).gameObject.SetActive(visibility);
	}
	
	
	/**
	 * Start 'speaking' the given text, removing the old content if any.
	 */
	public void say(string text) {
		textField.setMessage(text);
		textField.resetText();
		textField.startTyping();
		state = TutoState.Talking;
	}
	
	/**
	 * Get the tutorial on the screen...
	 */
	public void getIn() {
		LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(258, 152), 1.5f)
			.setEase(LeanTweenType.easeOutQuint);
		StartCoroutine(waitForReady());
	}
	
	private IEnumerator waitForReady() {
		yield return new WaitForSeconds(1.7f);
		
		state = TutoState.Ready;
		if(readyCallback != null)
			readyCallback();
	}
	
	public void getOut() {
		LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(-197, 152), 1.5f)
			.setEase(LeanTweenType.easeInQuint);
		state = TutoState.Hidden;
		StartCoroutine(waitForOut());
	}
	
	private IEnumerator waitForOut() {
		yield return new WaitForSeconds(1.7f);
		if(outCallback != null)
			outCallback();
	}
}