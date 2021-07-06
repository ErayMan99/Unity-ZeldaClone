using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableManager : MonoBehaviour {

	public AudioClip audioDestroy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void destroy()
	{
		SoundManager.instance.PlaySingle (audioDestroy);
		Destroy (this.gameObject);
	}
}
