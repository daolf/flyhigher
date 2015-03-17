using UnityEngine;
using System.Collections;

public class PrimaryCog : MonoBehaviour {

	public Sprite[] cogs;

	public SceneGeneratorScript generator;
	
	//public int difficulty = 0;
	
	private float rotationSpeed = 0;
	private float currentSpeed = 0;
	private float sinRatio = 1;
	
	private int cogId;

	// Use this for initialization
	void Start () {
		setCogId(Random.Range(0, cogs.Length));
	}
	
	/**
	 * Set the cog used from its ID
	 */
	public void setCogId(int id) {
		cogId = id;
		gameObject.GetComponent<SpriteRenderer>().sprite = cogs[cogId];
	}

	public int getCogId() {
		return cogId;
	}

	public void setSpeedRatio(float ratio) {
		sinRatio = Random.Range(1.0f, 3.0f) * ratio;
		rotationSpeed = Random.Range(40.0f, 90.0f) * ratio;
		rotationSpeed = Random.Range(0, 2) == 0 ? rotationSpeed : -rotationSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		// update rotation and speed (which is a sinusoid)
		currentSpeed = rotationSpeed * Mathf.Cos(Time.time / sinRatio);
		gameObject.transform.Rotate(0, 0, currentSpeed * Time.deltaTime);
	}

	void OnMouseUp() {
		if (generator.getCogToFind().getCogId () == cogId) {
			generator.cogFound();
		}
	}
}
