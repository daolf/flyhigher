using UnityEngine;
using System.Collections;

public class GuiScript : MonoBehaviour {

	public GUISkin customSkin;
	
	// Apply the Skin in our OnGUI() function
	public void OnGUI () {
				GUI.skin = customSkin;
		}
}
