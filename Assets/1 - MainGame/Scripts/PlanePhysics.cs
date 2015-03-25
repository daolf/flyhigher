using UnityEngine;
using System.Collections;

public class PlanePhysics : MonoBehaviour {

	public int reactorForce;
	private Vector2 origin;
	public float rotationSpeed;
	public Rigidbody2D rb;
	public float initialGravity;
	public Vector3 previousPos;
	private float bufferGravity;
	public SpriteRenderer mySprite;

	public enum FlappyState {BOUNCING, NORMAL};
	public FlappyState flappyState;

	//Bonus
	public enum BonusState{DOWNCLOUD, UPCLOUD, NONE};
	public BonusState bonusState;
	private float malusGravity;
	private float bonusGravity;

	public HeartBar myHeartBar;

	// Use this for initialization
	void Start () {
		flappyState = FlappyState.NORMAL;
		origin = transform.position;
		initialGravity = (float)0.3;
		previousPos = transform.position;

		bonusState = BonusState.NONE;
		bonusGravity = -(float)1.5;
		malusGravity = (float)1.5;

	}

	void OnEnable() {
		GetComponent<Rigidbody2D>().gravityScale = 1;
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		//Quand on appuie on alège la gravité
		if (flappyState == FlappyState.BOUNCING) {
			//print ("TouchMainGame");
			//TODO rendre plus linéaire on alors on met un maximum sur y (rb.gravityscale)

			// Pour rendre possible la remonté meme si on tombe trop vite
			if (Mathf.Abs(rb.velocity.y) > 0.5) {
				bufferGravity = (float)-1;
			}
			else {
				bufferGravity = (float)( 0.3 / -Mathf.Abs(rb.velocity.y));
			}


			if (bufferGravity > -2 ) {
				rb.gravityScale = bufferGravity;
			}

		}
		else {
			//print ("NoTouch");
			rb.gravityScale = initialGravity;
		}

		if(bonusState == BonusState.UPCLOUD)
			onGoodCloud();
		else if(bonusState == BonusState.DOWNCLOUD)
			onBadCloud();


		//To face direction
		transform.right = rb.velocity;
		//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);;
	}
	

	public void decoller (float angle, float power) {
		Quaternion theRotation = transform.localRotation;
		theRotation.eulerAngles = new Vector3(0,0,angle);
		transform.localRotation = theRotation;	

		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		GetComponent<Rigidbody2D>().AddForce(dir*power*(float)0.5);

	}

	public void setOrigin() {
		origin = transform.position;
	}

	public float getDistanceFromOrigin() {
		return Vector2.Distance(transform.position, this.origin);
	}

	public void onGoodCloud() {
		rb.gravityScale = bonusGravity;
	}

	public void onBadCloud() {
		rb.gravityScale = malusGravity;
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
