using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour {

	public CinemachineVirtualCamera [] cams;
	public GameObject player;
	[SerializeField]
	public Vector2 currentCameraPos;
	[SerializeField]
	public Vector2 currentPlayerPos;

	private int cameraId;
	public static CameraManager instance = null;     //Allows other scripts to call functions from SoundManager.             
	public bool teleport;

	void Awake ()
	{
		//Check if there is already an instance of SoundManager
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		//currentCameraPos = GameManager.instance.getScreenFromPosition (player.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		currentPlayerPos = GameManager.instance.getScreenFromPosition(player.transform.position);

		// If player is not on same screen than camera...
		if (currentCameraPos != currentPlayerPos) {

			// if we are not teleporting, change the camera ID so we get a smooth transition
			if (!teleport) {
				cameraId++;
			}
			teleport = false;

			// Update new camera position
			currentCameraPos = currentPlayerPos;
			GameManager.instance.changeScreen (currentCameraPos);

			// Set the camera at the right coord for the screen (bottom left)
			Vector3 coordCamera = new Vector3 (currentCameraPos.x * GameManager.instance.screenSize.x, currentCameraPos.y * GameManager.instance.screenSize.y, -1);
			// Offset to have camera in the center of the screen
			coordCamera += new Vector3 (GameManager.instance.screenSize.x / 2, Mathf.Ceil(GameManager.instance.screenSize.y / 2), 0f);
			cams [cameraId % 2].transform.position = coordCamera;

			// switch to new camera
			cams [cameraId % 2].Priority = 10;
			cams [cameraId % 2].MoveToTopOfPrioritySubqueue ();
		}
	}

	//void CheckPlayerPosition()
	//{
	//	// Calculate player pos
	//	currentPlayerPos = GameManager.instance.getScreenFromPosition(player.transform.position);
	//}
}
