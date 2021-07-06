using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpsExit : MonoBehaviour {

	private PlayerMovement player;
	private SpriteRenderer playerSprite;
	public AudioClip audioSteps;
	public GameObject destination;

	void Start(){
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerMovement> ();
		playerSprite = player.GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Player") && !GameManager.instance.cutscene) {
			CameraManager.instance.teleport = true;
			coll.gameObject.transform.position = destination.transform.position + (Vector3.down*0.75f);
			StartCoroutine ("warpExitCutscene");
		}
	}

	IEnumerator warpExitCutscene()
	{
		float timer = 0.75f;
		GameManager.instance.cutscene = true;
		playerSprite.sortingOrder = 1;
		player.animator.SetFloat ("Direction", 2f);
		player.animator.SetBool ("Moving", true);
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		player.GetComponent<PlayerMovement>().moveDirection =  Vector2.down;

		SoundManager.instance.PlaySingle (audioSteps);

		while (timer > 0) {
			timer -= Time.deltaTime;
			player.transform.position += new Vector3 (0, 1, 0) * Time.deltaTime;
			yield return null;
		}
		playerSprite.sortingOrder = 5;

		timer = 0.5f;
		while (timer > 0) {
			timer -= Time.deltaTime;
			player.transform.position += new Vector3 (0, -1, 0) * Time.deltaTime;
			yield return null;
		}

		GameManager.instance.cutscene = false;
		SoundManager.instance.musicSource.enabled = true;

	}
}
