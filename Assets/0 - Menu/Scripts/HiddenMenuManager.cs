using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HiddenMenuManager : MonoBehaviour {

	public ColorBlock lockedColors = ColorBlock.defaultColorBlock;
	
	public ColorBlock unlockedColors = ColorBlock.defaultColorBlock;
	
	public ColorBlock successedColors = ColorBlock.defaultColorBlock;

	// Use this for initialization
	void Start () {
	
	}
	
	public void Menu() {
		Application.LoadLevel("MainMenu");
	}
	
	public void LevelButtonPressed(Button button) {
		button.colors = unlockedColors;
	}
}
