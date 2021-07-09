using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JumpFeatherItem : MonoBehaviour
{
    public featherJumps[] featherJump;
	private GameObject player;
	private PlayerMovement playerMovement;
    private Vector3 position;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Tilemap tileMap;

    private float t;
    private int currentJump = 0;
    private float proportion;
    private float x;
    private float y;
    private float radius;
	private int jumpEnded;
	public GameObject shadow;
	private Vector3 waitLocation;

	//private enum state {jumping, waiting};
	private bool jumping = false;
	public float maxDelayBetweenJumps = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
		transform.position = player.transform.position;
        playerMovement = player.GetComponent<PlayerMovement> ();
        tileMap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
        // Préparons les 2 sauts
        foreach(featherJumps jump in featherJump)
        {
            //position = tileMap.WorldToCell(transform.position) + (Vector3) (playerMovement.moveDirection * jump.distance);
            //position = tileMap.CellToWorld(Vector3Int.RoundToInt(position));
			//Debug.Log("World to cell! " + position);

            //jump.box.transform.position = position;// + (Vector3) (Vector2.one/2);
            //jump.box.transform.position = Vector3Int.RoundToInt(transform.position) + (Vector3) (playerMovement.moveDirection * jump.distance);
            jump.box.transform.position = transform.position + (Vector3) (playerMovement.moveDirection * jump.distance);
            //Debug.Log("Round player " + jump.box.transform.position);
        }
		startJump();
    }

    // Update is called once per frame
    void Update()
    {
        if (!jumping && Input.GetKeyDown (KeyCode.J))
        {
            startJump();
        }

        if(jumping)
        {
            t += Time.deltaTime;            
            t = Mathf.Clamp (t, 0, featherJump[currentJump].jumpTime);
            proportion = t/featherJump[currentJump].jumpTime;
            x = radius-(radius*proportion*2);
            y = Mathf.Sqrt(Mathf.Pow(radius, 2f) - (x*x));
            player.transform.position = Vector3.Slerp (startPosition, endPosition, proportion);
			shadow.transform.position = player.transform.position+(Vector3.up/8)+(Vector3.right/2);
            player.transform.position += new Vector3(0,y,0);

			// End of current jump
            if(0.01f >= Mathf.Abs(Vector2.Distance(player.transform.position, endPosition)))
            {
				jumping = false;
				waitLocation = player.transform.position;
				shadow.SetActive(false);
                t = 0;
				currentJump++;
				Invoke("destroyIfNotJumped", maxDelayBetweenJumps);
            }
        }
		else
		{
			if (waitLocation != player.transform.position)
			Destroy (this.gameObject);
		}	
        if (currentJump >= featherJump.Length && !jumping)
        {
            Destroy (this.gameObject);
        }
    }

	void destroyIfNotJumped()
	{
		if (!featherJump[currentJump].done)
		{
			Destroy (this.gameObject);
		}
	}

	public void buttonPressed()
	{
		if (!jumping)
        {
			startJump();
        }
	}

	void startJump()
	{
		jumping = true;
		startPosition = player.transform.position;
		endPosition = featherJump[currentJump].box.transform.position;
		radius = featherJump[currentJump].jumpHeight;
		shadow.SetActive(true);
		CancelInvoke();
	}
}
