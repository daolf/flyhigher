using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public ChiffreScore unite;
	public ChiffreScore dizaine;
	public ChiffreScore centaine;
	public ChiffreScore millier;
	public int value;
	public ChiffreScore[] t;

	void OnGUI() {
		int i = 0;
		while (value > 0) {
			t[i].value = value % 10;
			value /= 10;
			i++;
		}

	}


	// Use this for initialization
	void Start () {
		value = 0;
		t = new ChiffreScore[4];
		t[0] = unite;
		t [1] = dizaine;
		t [2] = centaine;
		t [3] = millier;
	}



	// Update is called once per frame
	void Update () {
	
	}
}
