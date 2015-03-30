using UnityEngine;
using System.Collections;

public class SmoothTranslation : MonoBehaviour {
	public Vector3 from;
	public Vector3 to;
	public float duration;
	private float curTime;
	public SceneGeneratorScript sceneGenerator;

	// Use this for initialization
	void Start () {
		curTime = 0;
	}

	private float easeSineRatio(float ratio) {
		return  (-0.5f) * (Mathf.Cos (ratio * Mathf.PI) - 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (curTime < duration) {
			float ratio = easeSineRatio (curTime / duration);


			Vector3 newpos = gameObject.transform.position;
			newpos.x = from.x + (to.x - from.x) * ratio;
			newpos.y = from.y + (to.y - from.y) * ratio;

			gameObject.transform.position = newpos;
			curTime += Time.deltaTime;
		} else { // replace element 
			Vector3 newpos = gameObject.transform.position;
			newpos.x = from.x;
			newpos.y = from.y;
			gameObject.transform.position = newpos;
			sceneGenerator.isAnimEnd = true;
			curTime = 0;
			Destroy(this);
		}

		/*if (!sceneGenerator.hasPlayed) {
			sceneGenerator.isAnimEnd = true;//Delete
			curTime = 0;
		} else {
			sceneGenerator.isAnimEnd = false;
		}*/
	}
}
