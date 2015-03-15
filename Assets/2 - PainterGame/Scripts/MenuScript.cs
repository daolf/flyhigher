using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public void OnClickPlay() {
		print("ok");
		Application.LoadLevel ("PainterGameLvl0");
	}
}
