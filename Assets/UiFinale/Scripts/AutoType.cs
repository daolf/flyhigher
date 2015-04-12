using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoType : MonoBehaviour {
	
	public float letterPause;
	public bool autoStart = true;
	
	private string message;
	private IEnumerator currentCoroutine = null;
	private bool messageComplete = false;
	

	// Use this for initialization
	void Start () {
		message = GetComponent<Text>().text;
		resetText();
		if(autoStart)
			startTyping();
	}
	
	public bool isComplete() {
		return messageComplete;
	}
	
	// set a new message to display
	public void setMessage(string newMessage) {
		message = newMessage;
	}
	
	// reset the content of Text area to be able to type it again
	public void resetText() {
		GetComponent<Text>().text = "";
		messageComplete = false;
		// stop typing!
		if(currentCoroutine != null) {
			StopCoroutine(currentCoroutine);
			currentCoroutine = null;
		}
	}
	
	public void startTyping() {
		GetComponent<Text>().text = "";
		currentCoroutine = TypeText();
		StartCoroutine(currentCoroutine);
	}
	
	public void instantDisplay() {
		resetText();
		GetComponent<Text>().text = message;
		messageComplete = true;
	}
	
	IEnumerator TypeText () {
		foreach (char letter in message.ToCharArray()) {
			GetComponent<Text>().text += letter;
			yield return 0;
			yield return new WaitForSeconds(letterPause);
		}
		currentCoroutine = null;
		messageComplete = true;
	}
}