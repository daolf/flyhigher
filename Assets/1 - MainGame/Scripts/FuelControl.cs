using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuelControl : MonoBehaviour {

	public List<GameObject> fuelBars;
	public GameObject plane;

	private PlanePhysics planePhysic;
	float offsetProgress = 0.03f;
	private List<FuelBarScript> fuelBarBehaviors = new List<FuelBarScript>();

	bool empty { get {return fuelBarBehaviors.Count == 0;}}



	void Start () {
		planePhysic = plane.GetComponent<PlanePhysics>();
		foreach (GameObject behavior in fuelBars) {
			fuelBarBehaviors.Add(behavior.GetComponent<FuelBarScript>());
		}
	}
	
	void FixedUpdate () {
		if(Input.GetMouseButton(0) && !empty )
		{
			planePhysic.flappyState = PlanePhysics.FlappyState.BOUNCING;
			useFuel();
		}
		else
		{
			planePhysic.flappyState = PlanePhysics.FlappyState.NORMAL;
		}
	}


	void useFuel() {
		FuelBarScript fuelBarBehavior;
		if(fuelBarBehaviors.Count>0)
		{
			fuelBarBehavior = fuelBarBehaviors[fuelBarBehaviors.Count-1];
			fuelBarBehavior.progress -= offsetProgress;
			if(fuelBarBehavior.progress <= 0) {
				fuelBarBehavior.progress = 0;
				fuelBarBehaviors.Remove(fuelBarBehavior);
			}
		}
	}
}
