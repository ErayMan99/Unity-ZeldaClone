using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangAttack : MonoBehaviour {

	public Vector2 direction;
	private GameObject player;
	private Rigidbody2D rigidbody2D;
	public int dmg = 0;
	public float distance = 6f;
	public float speed = 6f;
	public bool isReturning;
	public int stunFrames = 120;
	public AudioClip audioHitWall;
	public AudioClip audioBoomerangLoop;
	private Vector2 startPosition;
	private Vector3 playerOffset = new Vector3(0.5f, 0.5f, 0);

	void Start()
	{
		rigidbody2D = gameObject.GetComponent<Rigidbody2D> ();
	}

	void OnEnable()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		//transform.parent = player.transform;
		direction = player.GetComponent<PlayerMovement> ().moveDirection;

		isReturning = false;
		transform.position = player.transform.position + playerOffset;
		startPosition = transform.position;

		SoundManager.instance.efxLoop.loop = true;
		SoundManager.instance.efxLoop.clip = audioBoomerangLoop;
		SoundManager.instance.efxLoop.Play();
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(0f, 0f, 30f);
		if (!isReturning)
			rigidbody2D.velocity = direction * speed;
		else
			rigidbody2D.velocity = ((player.transform.position + playerOffset) - transform.position).normalized * speed;
		// if far enough, return to sender
		if (Vector2.Distance (startPosition, transform.position) >= distance)
			isReturning = true;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		// If we hit a wall while going, we hit, then return. Nothing should happen if we hit a wall in the return.
		if (coll.gameObject.tag == "Wall" && !isReturning) {
			SoundManager.instance.PlaySingle (audioHitWall);
			isReturning = true;
		} 
		// If we hit an enemy either in the going or the return, we stun
		else if (coll.gameObject.tag == "Enemy") {
			Vector2 tempDirection = direction;
			// If we hit an enemy on the way back, reverse the hit direction so that it matches the new boomerang direction
			if (isReturning)
				tempDirection *= -1;
			coll.gameObject.GetComponent<EnemyMovement> ().Stun (tempDirection, stunFrames);
			isReturning = true;
		} 
		// If we hit an item pickup we initiate a return, but this is done in the item pickup class to avoid inconsistencies.

		// If we hit another projectile, for now we do nothing (ie the boomerang will destroy the projectile and keep going)
	}

	void OnTriggerStay2D(Collider2D coll){
		if (isReturning && coll.gameObject.tag == "Player") {
			SoundManager.instance.efxLoop.Stop ();
			Destroy (this.gameObject);
		}
	}
}
