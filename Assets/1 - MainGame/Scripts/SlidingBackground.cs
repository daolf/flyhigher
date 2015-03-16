using UnityEngine;
using System.Collections;

public class SlidingBackground : MonoBehaviour {

	public GameObject leftBackground, rightBackground;
	public GameObject mainCamera;
	public float xFactor;

	int swapGroundGrassNextTime;

	// Use this for initialization
	void Start () {
		swapGroundGrassNextTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		float cameraX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(mainCamera.GetComponent<Camera>().rect.min).x;
		Vector3 tmpPos;
		if(leftBackground.GetComponent<Renderer>().bounds.max.x < cameraX)
		{
			tmpPos = rightBackground.transform.position;
			tmpPos.x += rightBackground.GetComponent<Renderer>().bounds.size.x;
			leftBackground.transform.position = tmpPos;

			GameObject tmp = leftBackground;
			leftBackground = rightBackground;
			rightBackground = tmp;

			if(swapGroundGrassNextTime>0)
			{
				swapGroundGrass();
				swapGroundGrassNextTime--;
			}
		}
	}

	public void swapGroundOnNextFrame() {
		swapGroundGrassNextTime = 2;
	}

	void swapGroundGrass ()
	{
		SpriteRenderer rendererBG = rightBackground.GetComponent<SpriteRenderer>();
		
		rendererBG.sprite = Resources.Load<Sprite>("groundGrass");
		BoxCollider2D boxCollider = rightBackground.GetComponent<BoxCollider2D>();
		PolygonCollider2D polygonCollider = rightBackground.GetComponent<PolygonCollider2D>();
		
		boxCollider.enabled = false;
		polygonCollider.enabled = true;
	}
}
