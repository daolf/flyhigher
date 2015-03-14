using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {
	public GameObject targetTracked;
	float padding;
	void Start () {
		padding =  transform.position.x - targetTracked.transform.position.x;
	}
	
	void Update () {
		transform.position = new Vector3(targetTracked.transform.position.x + padding, transform.position.y, transform.position.z);
	}
}
