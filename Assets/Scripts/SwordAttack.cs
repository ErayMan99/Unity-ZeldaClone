using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour {

	public int dmg;
	public AudioClip audioSwordSlash;

	private Animator animator;
	private GameObject player;
	private Animator playerAnimator;
	private PlayerMovement playerMovement;

	[Header("Others")]
	public float swordCooldown;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMovement = player.GetComponent<PlayerMovement> ();
		
		Vector3 direction;

		switch ((int)GameManager.instance.getDirectionFromVector(playerMovement.moveDirection)) {
		case 0:
			direction = Vector3.up;
			break;
		case 1:
			direction = Vector3.left;
			break;
		case 2:
			direction = Vector3.down;
			break;
		case 3:
			direction = Vector3.right;
			break;
		default:
			direction = Vector3.zero;
			break;
		}
		
		transform.parent = player.transform;
		transform.position = player.transform.position + (Vector3)Vector2.one/2 + direction*0.7f;
		transform.Rotate(0f, 0f, 90f*GameManager.instance.getDirectionFromVector(playerMovement.moveDirection));			

		playerAnimator = player.GetComponent<Animator> ();
		playerAnimator.SetBool ("Sword", true);
		playerMovement = player.GetComponent<PlayerMovement> ();

		playSoundSwordSlash ();
		Invoke ("destroy", swordCooldown);
	}

	void destroy()
	{
		playerAnimator.SetBool ("Sword", false);
		Destroy (this.gameObject);
	}

	public void playSoundSwordSlash()
	{
		SoundManager.instance.PlaySingle (audioSwordSlash);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		// If we hit an obstacle, find another direction
		if (coll.gameObject.CompareTag ("Enemy")) {
			Vector2 pushback = Vector2.zero;
			//pushback = GameManager.instance.getVectorFromDirection (playerMovement.moveDirection);
			coll.gameObject.GetComponent<EnemyMovement> ().hit (playerMovement.moveDirection, dmg);
		}
	}
}
