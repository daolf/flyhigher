using UnityEngine;
using System.Collections;

public class PrimaryCog : MonoBehaviour {

	public Sprite[] cogs;
	
	public int difficulty;
	
	private float rotationSpeed;
	
	private int cogId;

	// Use this for initialization
	void Start () {
		setCogId(Random.Range(0, cogs.Length));
		
		rotationSpeed = Random.Range(40.0f, 90.0f) * difficulty;
		rotationSpeed = Random.Range(0, 2) == 0 ? rotationSpeed : -rotationSpeed;
	}
	
	/**
	 * Set the cog used from its ID
	 */
	void setCogId(int id) {
		cogId = id;
		gameObject.GetComponent<SpriteRenderer>().sprite = cogs[cogId];
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
	}
}
