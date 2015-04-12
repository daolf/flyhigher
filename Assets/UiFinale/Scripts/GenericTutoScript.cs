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
	
	public enum TutoState { Hidden, Ready, Talking, Waiting, Finish };
	private TutoState m_state = TutoState.Hidden;
	
	public TutoState state {
		get {
			return m_state;
		}
	}
	
	private IEnumerable<string> textsToSay = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( (m_state == TutoState.Talking || m_state == TutoState.Waiting)
		   && Input.GetButtonDown("Fire1"))
		{
			if(m_state == TutoState.Talking) {
				// instant display of the rest of the current message (coroutine handle that!)
				textField.instantDisplay();
			}
			else {
				m_state = TutoState.Talking;
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
		m_state = TutoState.Talking;
		
		foreach(string message in textsToSay) {
			textField.setMessage(message);
			textField.resetText();
			textField.startTyping();
			
			while(!textField.isComplete())
				yield return null;
				
			// wait before displaying next message
			m_state = TutoState.Waiting;
			//yield return new WaitForSeconds(0.1f);
			while(m_state == TutoState.Waiting)
				yield return null;
		}
		
		m_state = TutoState.Finish;
		if(dialogueEndCallback != null)
			dialogueEndCallback();
		else
			getOut();
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
		
		m_state = TutoState.Ready;
		if(readyCallback != null)
			readyCallback();
	}
	
	public void getOut() {
		LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector2(-197, 152), 1.5f)
			.setEase(LeanTweenType.easeInQuint);
		StartCoroutine(waitForOut());
	}
	
	private IEnumerator waitForOut() {
		yield return new WaitForSeconds(1.7f);
		m_state = TutoState.Hidden;
		if(outCallback != null)
			outCallback();
	}
}
