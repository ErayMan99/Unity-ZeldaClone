using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunic : MonoBehaviour
{
    public Color tunicColor;
    public int defenseMultiplicator = 1;

    private GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        player.GetComponent<SpriteRenderer>().material.SetColor("Color_Tunic", tunicColor);
        player.GetComponent<PlayerMovement>().defenseMultiplicator = defenseMultiplicator;
        Destroy (this.gameObject);
    }
}
