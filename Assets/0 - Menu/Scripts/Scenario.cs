﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scenario  {


	public int niveau;
	public int niveauMax = 9;
	public Etape[] scenario;

	// Use this for initialization
	public Scenario() {
		niveau = PlayerPrefs.GetInt (Constants.MAIN_GAME_LVL);
		scenario = new Etape[9];
		scenario[0] = new Etape(60,Constants.MECANO_GAME_LVL1_UNLOCK);
		scenario[1] = new Etape(80,Constants.PROPULSION_GAME_LVL1_UNLOCK);
		scenario[2] = new Etape(110,Constants.PAINT_GAME_LVL1_UNLOCK);
		scenario[3] = new Etape(120,Constants.MECANO_GAME_LVL2_UNLOCK);
		scenario[4] = new Etape(130,Constants.PROPULSION_GAME_LVL2_UNLOCK);
		scenario[5] = new Etape(180,Constants.PAINT_GAME_LVL2_UNLOCK);
		scenario[6] = new Etape(190,Constants.MECANO_GAME_LVL3_UNLOCK);
		scenario[7] = new Etape(200,Constants.PROPULSION_GAME_LVL3_UNLOCK);
		scenario[8] = new Etape(250,Constants.PAINT_GAME_LVL3_UNLOCK);
	}

	public void unlockNext(){
		if (niveau <= 9) {
			scenario [niveau].unlock ();
			niveau ++;
			PlayerPrefs.SetInt (Constants.MAIN_GAME_LVL,niveau);
		}
	}

	public int getObjectif() {
		if (niveau < scenario.Length) {
			return scenario [niveau].getObjectif ();
		} else {
			return -1;
		}
	}

	public string getGameUnlocked() {
		string r;
		if (scenario [niveau].getString ().Contains ("PAINT")) {
			r = "Peintre";
		} else if (scenario [niveau].getString ().Contains ("MECANO")) {
			r = "Mecano";
		} else if (scenario [niveau].getString ().Contains ("PROPUL")) {
			r = "Propulsion";
		} else {
			r = "Mystère";
		}

		return r;
	}

	public int getNiveauMax(){
		return niveauMax;
	}

	public bool isFinished(){
		return (niveau >= niveauMax);
	}
	/* Sorry for the uglyness of this*/
	public string getLevelUnlocked() {
		string r = "";
		if (!isFinished()) {
			if ( scenario [niveau].getString ().Contains ("LVL1")) {
				r = "1";
			} else if (scenario [niveau].getString ().Contains ("LVL2")) {
				r = "2";
			} else if (scenario [niveau].getString ().Contains ("LVL3")) {
				r = "3";
			} else {
				r = "Mystère";
			}
		}
		
		return r;
	}

}
