using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    private GameObject player;
	public AudioClip areaMusic;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag ("Player");

        CameraManager.instance.currentCameraPos = GameManager.instance.getScreenFromPosition (player.transform.position);
        GameManager.instance.Initialize();

        SoundManager.instance.musicSource.clip = areaMusic;
        SoundManager.instance.musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
