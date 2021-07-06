using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class featherJumps
{
	public AudioClip audio;
    public int distance;
    public GameObject box;
    public bool started;
    public bool done;
    public float jumpHeight;
    public float jumpTime;
}
