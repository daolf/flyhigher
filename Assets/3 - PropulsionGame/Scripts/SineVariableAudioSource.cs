using UnityEngine;
using System.Collections;

public class SineVariableAudioSource : MonoBehaviour {
	public float frequencyIn = 220;
	public float frequencyOut = 440;
	
	public float duration = 5;
	
	private int position = 0;
	private int sampleRate = 44100;
	private AudioClip internalAudioClip;

	// Use this for initialization
	void Start () {
		setDuration(duration);
		//source.Play();
	}
	
	public void setDuration(float newDuration) {
		duration = newDuration;
		// very, verrry ugly, but no other simple way! Thanks Unity.
		internalAudioClip = AudioClip.Create("VariableSinusoid", (int)(sampleRate*duration), 1,
		                                     sampleRate, true, audioReadCallback, audioSetPositionCallback);
		GetComponent<AudioSource>().clip = internalAudioClip;
	}

	void audioReadCallback(float[] data) {
		int count = 0;
		float lnfact = (float)duration / Mathf.Log(frequencyOut/frequencyIn);
		while (count < data.Length) {
			float currentTime = (float)position / (float)sampleRate;
			data[count] = Mathf.Sin(2 * Mathf.PI * frequencyIn * currentTime * Mathf.Exp(currentTime / lnfact));
			position++;
			count++;
		}
	}
	
	void audioSetPositionCallback(int position) {
		this.position = position;
	}
}
