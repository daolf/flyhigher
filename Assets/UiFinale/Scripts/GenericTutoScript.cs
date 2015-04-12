using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericTutoScript : MonoBehaviour {

	public AutoType textField;
	
	public delegate void StateChangeDelegate();
	public StateChangeDelegate dialogueEndCallback = null;
	public StateChangeDelegate readyCallback = null;
	public StateChangeDelegate outCallback = null;
	
	public TutoHandScript hand;
	
	private enum TutoState { Hidden, Ready, Talking, Waiting, Finish };
	TutoState state = TutoState.Hidden;
	
	private IEnumerable<string> textsToSay = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( (state == TutoState.Talking || state == TutoState.Waiting)
		   && Input.GetButtonDown("Fire1"))
		{
			if(state == TutoState.Talking) {
				// instant display of the rest of the current message (coroutine handle that!)
				textField.instantDisplay();
			}
			else {
				state = TutoState.Finish;
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
		string[] textArray = new string[1];
		textArray[0] = text;
		say(textArray);
	}
	
	public void say(IEnumerable<string> texts) {
		textsToSay = texts;
		StartCoroutine(coroutineSayAll());
	}
	
	public IEnumerator coroutineSayAll() {
		state = TutoState.Talking;
		
		foreach(string message in textsToSay) {
			textField.setMessage(message);
			textField.resetText();
			textField.startTyping();
			
			while(!textField.isComplete())
				yield return null;
				
			// wait before displaying next message
			yield return new WaitForSeconds(0.1f);
			while(!Input.GetButtonDown("Fire1"))
				yield return null;
		}
		
		state = TutoState.Waiting;
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
