using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour {

	[Header("Stats")]
	public float curHp;
	public float speed;
	protected bool moving;
	public int defenseMultiplicator = 1;

	[Header("Timers")]
	private int flashingFrames = 2;
	public Color flashColor;

	[Header("Components")]
	public Animator animator;
	public Vector2 moveDirection;
	protected SpriteRenderer renderer;
	protected Rigidbody2D rigidbody;
	public AudioClip audioHit;

	[Header("Push Back")]
	private Vector2 pushbackDirection;
	private int timerPushback;
	private int timerInvincibility;
	public int timerStun;
	public float pushbackForce = 10f;
	public int pushbackAmount = 10;
	public int invincibilityFrames = 30;

	// Use this for initialization
	public virtual void Start () {
		animator = this.GetComponent<Animator>();
		renderer = this.GetComponent<SpriteRenderer> ();
		rigidbody = this.GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	public virtual void Update () {
		
	}

	public virtual void FixedUpdate () {
		Vector2 tempVelocity = Vector2.zero;
		// Cutscene?
		if (GameManager.instance.cutscene) 
		{
			tempVelocity = Vector2.zero;
		} 
		// We move!
		else
		{
			// add movement velocity
			if (moving && timerStun <= 0)
				tempVelocity += moveDirection * speed;
			// add pushback velocity
			if (timerPushback > 0) {
				tempVelocity += pushbackDirection * pushbackForce;	
				timerPushback--;
			}
		}

		updateTimers ();
		updateInvincibility ();

		// Do the actual movement
		rigidbody.velocity = tempVelocity;
	}
	public void updateTimers()
	{
		// other timers
		if (timerInvincibility > 0)
			timerInvincibility--;
		if (timerStun > 0)
			timerStun--;
	}

	public void updateInvincibility ()
	{
		// Invincibility frames
		if (timerInvincibility > 0) {
			if (timerInvincibility % flashingFrames == 0) {
				// swap color
				if (renderer.color == flashColor)
					renderer.color = Color.white;
				else
					renderer.color = flashColor;
			}
		}
		else
			renderer.color = Color.white;
	}

	public virtual void hit(Vector2 direction, int dmg)
	{
		// Can only be hit while NOT in a cutscene or not in an invincibility frame
		if (!GameManager.instance.cutscene && timerInvincibility <= 0) {
			SoundManager.instance.PlaySingle (audioHit);
			curHp -= (float)dmg/defenseMultiplicator;

			timerInvincibility = invincibilityFrames;
			pushback (direction, pushbackAmount);
		}
	}

	public void pushback(Vector2 moveDirection, int amount)
	{
		// if not currently stunned
		if (timerStun <= 0) {
			timerPushback = amount;
			pushbackDirection = moveDirection;
		}
	}

	public virtual void addHealth(int amount)
	{
		curHp += (float)amount;
	}

	public virtual void Stun(Vector2 pushbackDir, int stun)
	{
		SoundManager.instance.PlaySingle (audioHit);
		// pushback first, then stun
		pushback (pushbackDir, pushbackAmount);
		timerInvincibility = invincibilityFrames;
		timerStun = stun;
	}
}
