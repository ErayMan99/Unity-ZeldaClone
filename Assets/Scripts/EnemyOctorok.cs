using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOctorok : EnemyMovement {
	public int projectileTimer;
	private GameObject projectile;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		projectile = transform.GetChild (0).gameObject;
		projectileTimer = Random.Range (180, 400);
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();
		if (isAbleToMove ()) {
			// Projectile
			if (projectileTimer <= 0) {
				if (!projectile.activeInHierarchy) {
					projectile.SetActive (true);
					projectileTimer = Random.Range (90, 400);
				}
			}
		}
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate ();
		if (projectileTimer > 0)
			projectileTimer--;
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		// If we hit an obstacle, find another direction
		if (coll.gameObject.tag == "Wall") {
			timerMovement = 0;
		}
	}	
}
