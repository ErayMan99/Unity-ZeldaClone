using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpsEntry : MonoBehaviour {

	private PlayerMovement player;
	private SpriteRenderer playerSprite;
	public AudioClip audioSteps;
	public GameObject destination;

	void Start()
	{
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerMovement> ();
		playerSprite = player.GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Player") && !GameManager.instance.cutscene) {
			StartCoroutine ("warpEnterCutscene");
		}
	}

	void warp()
	{
		CameraManager.instance.teleport = true;
		player.transform.position = destination.transform.position + (Vector3.left/2);
		SoundManager.instance.musicSource.enabled = false;
	}

	IEnumerator warpEnterCutscene()
	{
		float timer = 0.75f;
		GameManager.instance.cutscene = true;
		playerSprite.sortingOrder = 1;
		player.animator.SetFloat ("Direction", 0f);
		player.animator.SetBool ("Moving", true);
		player.GetComponent<PlayerMovement>().moveDirection =  Vector2.up;

		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

		SoundManager.instance.PlaySingle (audioSteps);

		while (timer > 0) {
			timer -= Time.deltaTime;

			player.transform.position += new Vector3 (0, -1, 0) * Time.deltaTime;
			yield return null;
		}
		warp ();
		playerSprite.sortingOrder = 5;

		timer = 0.5f;
		while (timer > 0) {
			timer -= Time.deltaTime;

			player.transform.position += new Vector3 (0, 1, 0) * Time.deltaTime;
			yield return null;
		}

		GameManager.instance.cutscene = false;
	}
}