using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour {

	public static int initializedSpawners = 0;

	//the amount of time it takes to make a max sized ball
	public const float MAX_HOLD_TIME = 0.5f;

	//layer mask for collision checks
	public LayerMask ownLayer;
	//an int for changing currently selected character
	public int playerNumber;

    //emblem on each button
    public Image buttonEmblem;

    //player light button animator
    public Animator playerLight;

	public string choice {get {return type;} set {type = value;}}

	//The position at which this button will spawn objects
	private Vector3 spawnPosition = Vector3.zero;
	//The float that keeps track of which way the ball goes
	private float directionModifier = 0.0f;

	//A string to keep track of which is the current ball type being used
	private string type = CharacterSelection.FIRE;
	//A bool that keeps track of whether or not the button is actually pressed
	private bool isPressed = false;
	//A bool to check whether of not this button is spawning objects
	private bool isSpawning = false;
	//The amount of time the player has been holding the button
	private float holdTime = 0.0f;
	//the gameobject that will show the preview of the ball
	private Rotator spawnPreview;

	// Use this for initialization
	void Awake () {

		//init spawn position
		CalculateSpawnPosition ();

		//create the spawn preview
		//CreateSpawnPreview ();

	}
	
	// Update is called once per frame
	void Update () {

		if (GameController.state != GameController.PLAYING) return;

		//if the current button is spawning
		//handle/increase the appropriate values
		if (isSpawning) {

			//calculate how long the player has been holding the button
			if (holdTime < MAX_HOLD_TIME)
				holdTime += Time.deltaTime;
			else
				holdTime = MAX_HOLD_TIME;

		}

		//update the visuals of the spawn preview
		UpdateSpawnPreview (holdTime);
		
	}

	//A function to initialize the spawn position
	void CalculateSpawnPosition () {

		//Get the "world space" coordinates of the rectangle's center
		spawnPosition = GetComponent<RectTransform>().TransformPoint (0,0,0);
		spawnPosition.z = 0.0f;

		//have to do this because unity is stupid and dont have a simple way to get screen
		//position of a UI element.
		float xSpawn = spawnPosition.x;
		float ySpawn = spawnPosition.y;

		//getting the correct screen position of button
		if (xSpawn != 0) {
			xSpawn = (Screen.width/3) * Mathf.Sign (xSpawn);
		} else {
			xSpawn = 0;
		}
			
		ySpawn = Screen.height * 0.31f * Mathf.Sign (ySpawn);

		//adding half the screen width and height to compensate for 0,0 of the screen being the bottom corner
		xSpawn += Screen.width/2;
		ySpawn += Screen.height/2;

		//convert the new x position back to world coordinates and finalize the spawn position
		//fucking rediculous that it had to be this complicated, 
		//there should have just been one built in function.
		Vector3 worldSpace = Camera.main.ScreenToWorldPoint (new Vector3 (xSpawn, ySpawn, 0));
		spawnPosition.x = worldSpace.x;
		spawnPosition.y = worldSpace.y;

		//calculate the direction modifier & player number
		directionModifier = - Mathf.Sign(spawnPosition.y);

		initializedSpawners++;
		if (initializedSpawners == 6) {
			transform.root.gameObject.SetActive (false);
		}

	}

	void CreateSpawnPreview () {
		spawnPreview = Instantiate <Rotator> (Resources.Load <Rotator> (type + "Preview"));
		spawnPreview.transform.position = spawnPosition;
	}

	//Initiate the spawning process
	public void BeginSpawning () {
		holdTime = 0.0f;
		isSpawning = true;

		//set the button as pressed
		isPressed = true;
	}

	//Update the spawn preview to relfect the current hold time
	public void UpdateSpawnPreview (float _holdTime) {

		//calculate the size of the preview
		float size = Ball.CalculateSizeByPercent (_holdTime/MAX_HOLD_TIME);
		spawnPreview.transform.localScale = Vector3.one * size;

		//check if the ball should be displayed
		if (ShouldDisplaySpawnPreview ((size/2)*1.05f)) {
			spawnPreview.gameObject.SetActive (true);
			spawnPreview.rotationSpeed = GetSpawnPreviewRotation () * directionModifier;

			//uncomment this code to automatically start spawning the ball while holdingin
			//without having to press the button again

//			if (isPressed) {
//				isSpawning = true;
//			}

		} else {
			spawnPreview.gameObject.SetActive (false);
			ResetSpawn ();
		}

	}

	public float GetSpawnPreviewRotation () {

		if (isPressed) {
			return Ball.CalculateRotationByPercent (holdTime/MAX_HOLD_TIME);
		} else {
			return Ball.MIN_SPAWN_ROTATION;
		}

	}

	bool ShouldDisplaySpawnPreview (float size) {
		return !Physics.CheckSphere(spawnPosition, size, ownLayer, QueryTriggerInteraction.Collide);
	}
		
	//End the spwning process
	public void EndSpawn () {
		if (isSpawning) {
			Spawn ();
		}
		ResetSpawn ();

		//set the button as pressed
		isPressed = false;
	}

	public void ResetSpawn () {
		holdTime = 0.0f;
		isSpawning = false;	
	}

	//Spawn a ball based on the current spawn values
	public void Spawn () {

		if (GameController.state != GameController.PLAYING) return;

		//create an instance of the ball prefab
		Ball b = Instantiate<Ball> (Resources.Load<Ball>(type));

		//give the ball the correct values
		b.transform.position = spawnPosition;

		//make the ball shoot in the right direction
		b.SetDirection (directionModifier);

		//make the ball shoot in the right direction
		b.SetGoal (-spawnPosition.y);

		//make the ball the proper size
		//feeding in the percentage of the total holdtime
		//that the button was held
		b.SetSize (holdTime/MAX_HOLD_TIME);

		//modify the speed
		b.SetSpeed (holdTime/MAX_HOLD_TIME);

		//set the initialrotation of the new ball
		b.rotator.transform.rotation = spawnPreview.transform.rotation;

		//set the ball to be on this spawner's layer mask
		//i do not know how this works but something something
		//layer masks are complicated for some reason haha
		b.gameObject.layer =  (int) Mathf.Log(ownLayer.value, 2);

        AnimateSpawnLight();

    }

    public void AnimateSpawnLight() {

        playerLight.SetTrigger("Flash");

    }

	public void LoadNewBall () {

		if (spawnPreview)
			Destroy (spawnPreview.gameObject);
		
		CreateSpawnPreview ();
		spawnPreview.transform.parent = this.transform.root;

		GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Buttons/" + type + " Button");

        buttonEmblem.sprite = Resources.Load <Sprite> ("Buttons/Emblems/" + type + " Emblem");

		//Continue in this fashion for the images on the buttons

		//Get a refernece to the image 
		//Get a reference to the icon
		//Set the image to resources.load the corresponding button image
		//Set the icon to resources.load the corresponing icon

		//it would also be a good idea here to load all the necessary resources into memory
		//to avoid lag

	}

}
