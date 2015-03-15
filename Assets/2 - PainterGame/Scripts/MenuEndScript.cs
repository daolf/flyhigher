using UnityEngine;
using System.Collections;

public class MenuEndScript : MonoBehaviour {
	
	public void OnClickReplay() {
		Application.LoadLevel ("PainterGameLvl0");
	}
	public void OnClickMenuPrincipal() {
		print ("Menu principal à faire");
	}
}
