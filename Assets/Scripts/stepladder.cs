using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class stepladder : MonoBehaviour
{
    private Tilemap waterTilemap;
    // Start is called before the first frame update
    void Start()
    {
        waterTilemap = GameObject.FindGameObjectWithTag("Water").GetComponent<Tilemap>();
        waterTilemap.SetColliderType(waterTilemap.WorldToCell(transform.position), Tile.ColliderType.None);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        Destroy (this.gameObject);
		if (coll.gameObject.tag == "Player") {
            waterTilemap.SetColliderType(waterTilemap.WorldToCell(transform.position), Tile.ColliderType.Sprite);
			Destroy (this.gameObject);
		}
    }
}
