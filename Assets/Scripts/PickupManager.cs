using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {
	public static PickupManager instance = null;     //Allows other scripts to call functions from SoundManager.             

	public GameObject[] pickups;

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

	public void createRandomPickup(Vector3 position)
	{
		if (0 == Random.Range(0, 1)){
			// Decide what pickups in any
			int item = Random.Range(0, pickups.Length);
			GameObject newPickup = Instantiate (pickups [item]);
			newPickup.transform.position = position + Vector3.one/2;
		}
	}
}
