using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

	public Sprite lvl0;
	public Sprite lvl1;
	public Sprite lvl2;
	public Sprite lvl3;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt(Constants.PAINT_GAME_LVL4_SUCCES) == 1 ) {
			GetComponent<SpriteRenderer>().sprite = lvl3;
		}
		else if (PlayerPrefs.GetInt(Constants.PAINT_GAME_LVL3_SUCCES) == 1 ) {
			GetComponent<SpriteRenderer>().sprite = lvl2;
		}
		else if (PlayerPrefs.GetInt(Constants.PAINT_GAME_LVL2_SUCCES) == 1 ) {
			GetComponent<SpriteRenderer>().sprite = lvl1;
		}
		else {
			GetComponent<SpriteRenderer>().sprite = lvl0;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
