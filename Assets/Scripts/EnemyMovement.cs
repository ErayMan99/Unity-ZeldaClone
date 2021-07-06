using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : BaseMovement {

	public AudioClip audioDie;
	public int timerMovement;
	public int dmg = 1;
	public int maxHp = 10;
	public Vector2 startPosition;
	protected Collider2D collider;

	void Awake(){
		startPosition = transform.position;
		collider = this.GetComponent<Collider2D> ();
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
		curHp = maxHp;
		speed += Random.Range (0, 2);
	}

	public virtual void OnEnable()
	{
		// Put the enemy back to where it started
		transform.position = startPosition;
		moveDirection = Vector2.zero;
		timerMovement = 0;
		curHp = maxHp;
		collider.enabled = true;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (isAbleToMove()) {
			//Movement
			// If not moving, either pause or find another direction
			if (timerMovement <= 0) {
				moveDirection = GameManager.instance.getVectorFromDirection (Random.Range (0, 5));
				timerMovement = Random.Range (32, 64);

				animator.SetBool ("moving", true);

				if (moveDirection == Vector2.zero) {
					moving = false;
					animator.SetBool ("moving", false);
				}
				else {
					moving = true;
					animator.SetFloat ("Direction", GameManager.instance.getDirectionFromVector (moveDirection));
				}
			}

			// If enemy died, stop it's movement
			if (curHp <= 0) {
				moveDirection = Vector2.zero;
			}
		}
		else{
			moving = false;
			animator.SetBool ("moving", false);
		}
	}
	public bool isAbleToMove()
	{
		return (!GameManager.instance.cutscene && timerStun <= 0 && GameManager.instance.isCurrentScreen (transform.position));
	}
	public override void hit(Vector2 direction, int dmg)
	{
		if (!animator.GetBool("Death"))
		{
			curHp -= dmg;
			// Enemy dies?
			if (curHp <= 0) {
				SoundManager.instance.PlaySingle (audioDie);
				animator.SetTrigger ("Death");
				Invoke ("Death", 0.35f);
				collider.enabled = false;
			} 
			// regular hit
			else {
				base.hit (direction, 0);
			}
		}
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate ();
		if (timerMovement > 0)
			timerMovement--;
	}

	public void Death ()
	{
		PickupManager.instance.createRandomPickup (this.transform.position);
		this.gameObject.SetActive (false);
	}

	public override void Stun(Vector2 pushbackDir, int stun)
	{
		base.Stun (pushbackDir, stun);
		// Stop planned movement
		timerMovement = 0;
	}
}
