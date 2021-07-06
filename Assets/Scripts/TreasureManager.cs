using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureManager : MonoBehaviour {

	public GameObject treasure;
	private bool taken;
	private GameObject player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Player")) {
			player = GameObject.FindGameObjectWithTag("Player");
			player.GetComponent<PlayerMovement>().addItem(treasure);
			GameManager.instance.getTreasure (this.gameObject);
			treasure.SetActive (true);
			Invoke ("destroy", 2f);
		}
	}

	private void destroy()
	{
		Destroy (this.gameObject);
	}
}
