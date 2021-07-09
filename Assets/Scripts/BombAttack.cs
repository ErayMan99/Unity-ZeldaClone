using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack : MonoBehaviour {

	public float radius = 1f;
	public int dmg = 1;
	public float timeToExpode = 1.5f;
	public AudioClip audioExplode;
	public AudioClip audioDrop;

	private Animator anim;
	private GameObject player;
	private PlayerMovement playerMovement;

	// Use this for initialization
	void Start () {
		anim = this.gameObject.GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMovement = player.GetComponent<PlayerMovement> ();
		transform.position = player.transform.position + (Vector3.one/2) + (Vector3)playerMovement.moveDirection;
		SoundManager.instance.PlaySingle (audioDrop);
		Invoke ("explode", timeToExpode);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 1f);
        //Gizmos.DrawSphere(transform.position, 1);
		Gizmos.DrawWireSphere(transform.position, radius);
    }	

	public void explode()
	{
		Collider2D [] listColliders;
		listColliders = Physics2D.OverlapCircleAll ((Vector2)transform.position, radius);

		anim.SetTrigger ("Explode");
		SoundManager.instance.PlaySingle (audioExplode);

		foreach (var coll in listColliders) {
			if (coll.gameObject.CompareTag ("Enemy")) {
				coll.gameObject.GetComponent<BaseMovement>().hit((coll.gameObject.transform.position - transform.position).normalized, dmg);
			}
			else if (coll.gameObject.CompareTag ("Player")) {
				coll.gameObject.GetComponent<BaseMovement>().hit((coll.gameObject.transform.position - transform.position).normalized, dmg);
			}
			else if (coll.gameObject.CompareTag ("Destructable")) {
				coll.gameObject.GetComponent<DestructableManager>().destroy();
			}
		}
	}

	public void destroy()
	{
		Destroy (this.gameObject);
	}
}
