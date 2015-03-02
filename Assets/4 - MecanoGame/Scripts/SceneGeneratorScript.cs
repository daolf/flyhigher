using UnityEngine;
using System.Collections;

public class SceneGeneratorScript : MonoBehaviour {

	public const int COGS_NB = 8;

	public PrimaryCog[] cogs = new PrimaryCog[COGS_NB];
	public PrimaryCog cogToFind;

	// Use this for initialization
	void Start () {
		cogToFind.setCogId(Random.Range(0, cogs.Length));
		
		// initialize all cogs with "random" ids (in fact each one need to be uniq, so its a shuffle)
		int[] cogIds = new int[COGS_NB];
		for(int i=0; i<COGS_NB; i++)
			cogIds[i] = i;
		// swap 20 times
		for(int step=0; step<20; step++) {
			int a = Random.Range(0, COGS_NB);
			int b = Random.Range(0, COGS_NB);
			int swap = cogIds[a];
			cogIds[a] = cogIds[b];
			cogIds[b] = swap;
		}
		
		// set cog ids and speed
		for(int i=0; i<COGS_NB; i++) {
			cogs[i].setCogId(cogIds[i]);
			cogs[i].setSpeedRatio(3.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
