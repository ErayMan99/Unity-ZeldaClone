using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_pickups : MonoBehaviour {

	public float timeBeforeFlash = 8f;
	public float timeEachFlash = 0.1f;
	public float timeBeforeDestroy = 12f;
	private PlayerMovement playerScript;
	public AudioClip audioPickup;
	private SpriteRenderer renderer;
	public int pickupValue;

	// Use this for initialization
	void Start () {
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
		renderer = this.GetComponent<SpriteRenderer> ();
		InvokeRepeating ("flashing", timeBeforeFlash, timeEachFlash);
		Destroy (this.gameObject, timeBeforeDestroy);
	}

	void flashing()
	{
		renderer.enabled = !renderer.enabled;
	}
		
	void OnTriggerEnter2D(Collider2D coll)
	{
		// If we hit an obstacle, find another direction
		if (coll.gameObject.CompareTag ("Player") || coll.gameObject.CompareTag ("Sword"))
		{
			// Heart
			SoundManager.instance.PlaySingle (audioPickup);
			if (this.gameObject.CompareTag("Heart"))
				playerScript.addHealth(pickupValue);
			if (this.gameObject.CompareTag ("Rupee"))
				GameManager.instance.addRupee (pickupValue);
			Destroy (this.gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Boomerang")) {
			transform.position = coll.transform.position;
			// initiate the boomerang return (done here to avoid having the boomerang return before this hit registers
			coll.gameObject.GetComponent<BoomerangAttack>().isReturning = true;
		}
	}
}
