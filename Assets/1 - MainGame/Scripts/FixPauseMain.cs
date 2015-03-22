using UnityEngine;
using System.Collections;

public class FixPauseMain : MonoBehaviour {

	public MainGame mainGame;
	// Pour régler le soucis du clic durant le décollage
	public void rollBackState () {
		if (mainGame.state == MainGame.State.INTRO) {
				if (mainGame.scriptIntroControl.state ==
			    	IntroControl.State.ONECLICK) {
				mainGame.scriptIntroControl.state = IntroControl.State.INIT;
			}else if (mainGame.scriptIntroControl.state ==
			     IntroControl.State.TWOCLICK) {
				mainGame.scriptIntroControl.state = IntroControl.State.ONECLICK;
			}
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
