using UnityEngine;
using System.Collections;

public class Etape {

	public int _objectif;
	public string _unlock;
	public bool _isUnlock;

	public Etape(int objectif,string unlock) {
		this._objectif = objectif;
		this._unlock = unlock;
		_isUnlock = false;
	}

	public void unlock(){
		PlayerPrefs.SetInt (_unlock, 1);
		_isUnlock = true;
	}

	public int getObjectif() {
		return _objectif;
	}
}
