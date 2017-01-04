using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Ball : MonoBehaviour {

	//the lowest amount of damage that a ball can do
	public const float MIN_DAMAGE = 0.3f;

	//The smallest size a ball can be spawned
	public const float MIN_SPAWN_SIZE = 1.3f;
	//The largest size a ball can be spawned
	public const float MAX_SPAWN_SIZE = 3.9f;
	//The absolute largest a ball can be ever, ever
	//This is mainly to stop ball size from getting 
	//tooo rediculous
	public const float ABSOLUTE_MAX_SIZE = 8.0f;

	//The slowest speed a ball can be spawned
	public const float MIN_SPAWN_SPEED = 2.0f;
	//The fastest speed a ball can be spawned
	public const float MAX_SPAWN_SPEED = 8.0f;
	//The absolute slowest a ball can be ever, ever
	//This is mainly to stop ball speed from going to zero
	public const float ABSOLUTE_MIN_SPEED = 0.5f;

	//this will be a percentage that will ultimately control the size of the ball
	//and determine whether or not it should be destroyed
	[HideInInspector]
	public float health;

	//-1 or 1, will determine direction
	[HideInInspector]
	public float directionModifier;

	//a list of the balls that this has collided with this frame
	[HideInInspector]
	public List<Ball> ignoreCollision;

	//how fast will the ball go
	private float speed;

	//the y value of the end zone
	private float goal;
	
	// Update is called once per frame
	void Update () {

		if (GameController.state != GameController.PLAYING) return;

		//clear ignore collision list if it contains anything
		if (ignoreCollision.Count > 0)
			ignoreCollision.Clear ();
			
		//move the ball in the correct direction
		MoveForward ();

	}

	void MoveForward () {

		float deltaSpeed = speed * Time.deltaTime;
		transform.Translate (0.0f, deltaSpeed, 0.0f);

	}

	//functions to determine which direction the ball shoots
	public void SetDirection (float newDirection) {
		directionModifier = newDirection;
		speed *= newDirection;
	}

	//functions to determine which direction the ball shoots
	public void SetGoal (float newGoal) {
		goal = newGoal;
	}

	//a function to set the health of the ball and the convert that into real world scale
	//then it calculates speed based on that
	public void SetSize (float percent) {

		//set the health equal to the percent
		//this will make it much easier to keep track
		//of the balls being destroyed
		health = percent;

		//check if he ball should be killed
		if (this.IsDead ()) {
			Destroy (gameObject);
			return;
		}

		//find the size corresponding to the current health
		float size = CalculateSizeByPercent (percent);

		//Set the correct scale 
		transform.localScale = Vector3.one * size;

	}

	//this function will also be used for determining the preview size
	public static float CalculateSizeByPercent (float percent) {

		//determine the size based on the percentage of max hold time
		//that the player held the button
		float size = MIN_SPAWN_SIZE;
		size += (MAX_SPAWN_SIZE - MIN_SPAWN_SIZE) * percent;

		if (size > ABSOLUTE_MAX_SIZE)
			size = ABSOLUTE_MAX_SIZE;

		return size;
		
	}

	public void SetSpeed (float percent) {

		//determine the speed based on the percentage of max hold time
		//that the player held the button
		float newSpeed = MAX_SPAWN_SPEED;
		newSpeed -= (MAX_SPAWN_SPEED - MIN_SPAWN_SPEED) * percent;

		if (newSpeed < ABSOLUTE_MIN_SPEED)
			newSpeed = ABSOLUTE_MIN_SPEED;

		//Set the correct speed 
		speed = newSpeed * directionModifier;
		
	}

	public bool isFriendlyWith (Ball otherBall) {
		return otherBall.directionModifier == this.directionModifier;
	}

	void CalculateHitWith (Ball otherBall) {

		ShowImpactWith (otherBall);
		float ownHealth = this.health;

		this.health -= otherBall.health + MIN_DAMAGE;
		otherBall.health -= ownHealth + MIN_DAMAGE;

		this.CalculateDamageResult ();
		otherBall.CalculateDamageResult ();
		
	}

	void CalculateFusionWith (Ball otherBall) {
		
		if (directionModifier * this.transform.position.y >= directionModifier * otherBall.transform.position.y) {
			this.Absorb (otherBall);

		} else {
			otherBall.Absorb (this);
		
		}
		
	}

	void Absorb (Ball otherBall) {

		this.health += otherBall.health;
		this.SetSize (this.health);
		this.SetSpeed (this.health);

		Destroy (otherBall.gameObject);

	}

	bool IsDead () {
		return this.health < 0.0f;
	}

	void CalculateDamageResult () {
		
		if (this.IsDead ()) {
			Destroy (this.gameObject);
		} else {
			this.SetSize (this.health);
		}

	}

	void ShowImpactWith (Ball otherBall) {

		float higherHealth = 0.0f;

		if (this.health > otherBall.health) {
			higherHealth = this.health;
		} else {
			higherHealth = otherBall.health;
		}

		CameraShake2D.instance.ShakeCamera (higherHealth/10f, higherHealth/3f, 1f);

	}

	public void DestroyAllBallsExceptThis () {

		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");

		for (int i = 0; i < balls.Length; i++) {

			if (balls [i] != this.gameObject)
				Destroy (balls[i]);
			
		}

	}

	public void WinGame () {

		if (goal > 0)
			GameController.instance.score.IncreaseP1Score ();
		else
			GameController.instance.score.IncreaseP2Score ();

		//Do all the rest of the win game logic
		CameraShake2D.instance.ShakeCamera (0.3f, 2f, 1f);
		GameController.instance.EndGame ();
		Destroy (gameObject);
	}

    void MediumScreenShake() {
        CameraShake2D.instance.ShakeCamera(0.5f, 0.1f, 1f);
    }

    void BigScreenShake() {
        CameraShake2D.instance.ShakeCamera(0.5f, 0.3f, 1f);
    }

	public void OnTriggerEnter (Collider other) {

		if (other.gameObject.CompareTag ("Ball")) {

			//Get a reference to the other ball
			Ball otherBall = other.GetComponent <Ball> ();

			//skip the collision check if the ball has already been collided with
			for (int i = 0; i < ignoreCollision.Count; i++) {
				if (ignoreCollision[i] == otherBall)
					return;
			}

			//calculate the collision
			if (otherBall.isFriendlyWith (this)) {
				CalculateFusionWith (otherBall);
			} else {
				CalculateHitWith (otherBall);
			}

			//add this to the list of ball that the other should ignore collisions with
			otherBall.ignoreCollision.Add (this);

		} else if (other.gameObject.CompareTag ("EndZone")) {

			DestroyAllBallsExceptThis ();
			GameController.state = GameController.OVER;
			Invoke ("WinGame", 1);

            MediumScreenShake();
            Invoke("BigScreenShake", 0.5f);
			
		}
			
	}

}
