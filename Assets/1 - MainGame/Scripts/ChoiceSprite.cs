using UnityEngine;
using System.Collections;

public class ChoiceSprite : MonoBehaviour {

	public Sprite lvl0;
	public Sprite lvl1;
	public Sprite lvl2;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt(Constants.PAINT_GAME_LVL3_UNLOCK) == 1 ) {
			GetComponent<SpriteRenderer>().sprite = lvl2;
		}
		else if (PlayerPrefs.GetInt(Constants.PAINT_GAME_LVL2_UNLOCK) == 1 ) {
			GetComponent<SpriteRenderer>().sprite = lvl1;
		}
		else  {
			GetComponent<SpriteRenderer>().sprite = lvl0;
		}
		Destroy(GetComponent<PolygonCollider2D>());
		gameObject.AddComponent<PolygonCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
