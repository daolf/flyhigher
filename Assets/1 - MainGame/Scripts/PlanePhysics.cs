using UnityEngine;
using System.Collections;

public class PlanePhysics : MonoBehaviour {

	public int reactorForce;
	private Vector2 origin;
	public float rotationSpeed;
	public Rigidbody2D rb;
	public Vector3 previousPos;
	private float bufferGravity;
	public SpriteRenderer mySprite;

	public enum FlappyState {BOUNCING, NORMAL};
	public FlappyState flappyState;

	//Bonus
	public enum BonusState{DOWNCLOUD, UPCLOUD, NONE};
	public BonusState bonusState;

	public HeartBar myHeartBar;
	
	private const float GRAVITY_APPROX = 9.8f * 0.3f;
	
	// decrease speed value (drag) : high value -> high decrease
	private const float PLANE_AIR_DRAG = 0.02f;
	
	// boost in horizontal speed provided by good cloud wind (newton per second)
	private const float WIND_BOOST_VALUE = 5.0f;
	
	// vertical boost (or malus) provided by clouds (newton per second)
	private const float CLOUD_BOOST_VALUE = GRAVITY_APPROX*6.0f;
	
	// vertical boost for bouncing when using fuel
	private const float FUEL_BOOST_VALUE = GRAVITY_APPROX*4.3f;

	// Use this for initialization
	void Start () {
		flappyState = FlappyState.NORMAL;
		origin = transform.position;
		previousPos = transform.position;

		bonusState = BonusState.NONE;
	}

	void OnEnable() {
		GetComponent<Rigidbody2D>().gravityScale = 0.3f;
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		//Quand on appuie on alège la gravité
		if (flappyState == FlappyState.BOUNCING) {
			//print ("TouchMainGame");
			//TODO rendre plus linéaire on alors on met un maximum sur y (rb.gravityscale)

			// provide a better control when the plane is more or less horizontal
			if (Mathf.Abs(rb.velocity.y) < 0.5) {
				float ratio = (1.0f - Mathf.Abs(rb.velocity.y)) * 2.0f;
				rb.AddForce (Vector2.up*FUEL_BOOST_VALUE*ratio*rb.mass, ForceMode2D.Force);
			}
			else {
				rb.AddForce (Vector2.up*FUEL_BOOST_VALUE*rb.mass, ForceMode2D.Force);
			}
		}

		if(bonusState == BonusState.UPCLOUD)
			onGoodCloud();
		else if(bonusState == BonusState.DOWNCLOUD)
			onBadCloud();


		//To face direction
		transform.right = rb.velocity;
		//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);;
		//print(GetComponent<Rigidbody2D>().velocity.x);
	}
	

	public void decoller (float angle, float power) {
		Quaternion theRotation = transform.localRotation;
		theRotation.eulerAngles = new Vector3(0,0,angle);
		transform.localRotation = theRotation;	

		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		rb.AddForce(dir*power*(float)0.5);
		rb.drag = PLANE_AIR_DRAG;

	}

	public void setOrigin() {
		origin = transform.position;
	}

	public float getDistanceFromOrigin() {
		return Vector2.Distance(transform.position, this.origin);
	}

	public void onGoodCloud() {
		rb.AddForce (Vector2.up*CLOUD_BOOST_VALUE*rb.mass, ForceMode2D.Force);
		rb.AddForce (Vector2.right*WIND_BOOST_VALUE, ForceMode2D.Force);
	}

	public void onBadCloud() {
		rb.AddForce (-Vector2.up*CLOUD_BOOST_VALUE*rb.mass, ForceMode2D.Force);
	}

	IEnumerator myBlink() {
		//Vraiment désolé mais j'ai essayé avec des boucles, j'ai pas réussi , ça ma saoulé
		mySprite.enabled = false;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = true;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = false;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = true;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = false;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = true;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = false;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = true;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = false;
		yield return new WaitForSeconds ((float)0.1);
		mySprite.enabled = true;
	}

	public void Blink(){
		StartCoroutine("myBlink");

	}

	public void handleMongolfiere(){
		myHeartBar.looseLife ();
		Blink ();
		//On clignote
	}



}
