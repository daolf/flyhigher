using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour {

	public int maxLife;
	public int currLife;
	public Heart h1;
	public Heart h2;
	public Heart h3;
	private Heart[] heartBar;
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt (Constants.MAIN_GAME_MAX_LIFE,2);
		maxLife = PlayerPrefs.GetInt (Constants.MAIN_GAME_MAX_LIFE);
		currLife = maxLife;
		heartBar = new Heart[3];
		heartBar [0] = h1;
		heartBar[1] = h2;
		heartBar[2] = h3;
		for (int i=0; i<maxLife; i++) {
			heartBar[i].GetComponent<Image>().enabled = true;
		}
	}

	void OnGUI() {
		for (int i=0; i<3; i++) {
			if (heartBar[i].GetComponent<Image>().enabled == true){
				if (i<currLife) {
					heartBar[i].full = true;
				}
				else {
					heartBar[i].full = false;
				}
			}
		}
	}

	public void looseLife(){
		currLife -= 1;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
