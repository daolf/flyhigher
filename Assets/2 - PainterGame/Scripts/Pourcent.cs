using UnityEngine;
using System.Collections;

public class Pourcent : MonoBehaviour {

	public ChiffreScore unite;
	public ChiffreScore dizaine;
	public ChiffreScore centaine;
	public int value;
	public ChiffreScore[] t;
	
	void OnGUI() {
		if (value > 100) {
			value = 100;
		}
		int i = 0;
		int temp = value;
		if (value == 0) {
			for (i=0;i<3;i++) {
				t[i].value = 0;
			}
		}
		i = 0;
		while (i < 3) {
			if (temp > 0)
			{
				t[i].value = temp % 10;
				temp = temp/10;
			}
			else {
				t[i].value = 0;
			}
			i++;
		}
	}
	
	
	// Use this for initialization
	void Start () {
		value = 0;
		t = new ChiffreScore[3];
		t[0] = unite;
		t [1] = dizaine;
		t [2] = centaine;
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
