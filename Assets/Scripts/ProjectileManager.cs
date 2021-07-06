using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

	public Vector2 direction;
	public Animator parentAnimator;
	private Rigidbody2D rigidbody2D;
	public float speed = 1;
	public int dmg = 1;

	void Start()
	{
		//parentAnimator = gameObject.GetComponentInParent<Animator> ();
			//transform.parent.gameObject.GetComponent<Animator> ();
		rigidbody2D = gameObject.GetComponent<Rigidbody2D> ();
	}

	void OnEnable()
	{
		transform.position = transform.parent.transform.position;
		direction = GameManager.instance.getVectorFromDirection (parentAnimator.GetFloat("Direction"));
		Invoke ("disable", 3f);
	}

	// Update is called once per frame
	void FixedUpdate () {
		rigidbody2D.velocity = direction * speed;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<PlayerMovement> ().hit (direction, dmg);
			gameObject.SetActive (false);
		} else if (coll.gameObject != transform.parent.gameObject) {
			gameObject.SetActive (false);
		}
	}

	void disable()
	{
		gameObject.SetActive (false);
	}

}
