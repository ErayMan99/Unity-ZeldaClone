using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerMovement : BaseMovement {
    public static PlayerMovement instance = null;  
	
	[Header("Inventory")]
	public GameObject sword;
	public GameObject boomerang;
	public GameObject bomb;
	public GameObject stepladder;
	public GameObject feather;
	public GameObject tunic;
	private GameObject currentTunic;

	[Header("Components")]
	private Animator swordAnimator;
	private SwordAttack swordScript;

	[Header("Tilemaps")]
 	public Tilemap waterTilemap;
	
	void Awake ()
	{
		//Check if there is already an instance of SoundManager
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	public override void Start () {
		currentTunic = tunic;
		base.Start ();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();

		if(currentTunic != tunic)
		{
			currentTunic = tunic;
			Instantiate (tunic);
		}	

		// hack to increase hearts
		if (Input.GetKeyDown (KeyCode.P)) {
			curHp++;
			GameManager.instance.UpdateHearts ();
		}

		if(Input.GetKeyDown (KeyCode.D)) 
		{
			StartCoroutine(ChangeScene("Dungeon1"));
		}

		if(Input.GetKeyDown (KeyCode.F)) 
		{
			StartCoroutine(ChangeScene("LegendOfZelda"));
		}

		// Inputs
		if (!GameManager.instance.cutscene) {
			Vector2 newDirection = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

			moving = false;
			animator.SetBool ("Moving", moving);

			// Movement + attack can only be initiated when not currently attacking
			if (GameObject.FindGameObjectsWithTag("Sword").Length == 0)
			{
				// Movement
				if (Vector2.zero != newDirection) {
					moving = true;
					moveDirection = newDirection;
				}
				animator.SetFloat ("Direction", GameManager.instance.getDirectionFromVector(moveDirection));
				animator.SetBool ("Moving", moving);
				
				// Sword
				
				if(Input.GetKeyDown (KeyCode.Space) && sword != null)
				{
					//if (!boomerang.activeInHierarchy)
					//	boomerang.SetActive (true);
					if(GameObject.FindGameObjectsWithTag("Sword").Length == 0)
					{
						Instantiate (sword);
					}	
				}

				// Boomerang
				if(Input.GetKeyDown (KeyCode.B) && boomerang != null) 
				{
					if(GameObject.FindGameObjectsWithTag("Boomerang").Length < 30)
						Instantiate (boomerang);
				}

				// Bomb
				if(Input.GetKeyDown (KeyCode.V) && bomb != null) 
				{
					if(GameObject.FindGameObjectsWithTag("Bomb").Length < 30)
						Instantiate (bomb);
				}

				// Feather Jump
				if(Input.GetKeyDown (KeyCode.J) && feather != null) 
				{
					if(GameObject.FindGameObjectsWithTag("Feather").Length == 0)
						Instantiate (feather);
				}
				
			}
		}
	}

	public override void FixedUpdate () {
		base.FixedUpdate ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.CompareTag("Enemy")) {
			EnemyMovement enemy = coll.gameObject.GetComponent<EnemyMovement> ();
			// Only does damage if the enemy is not stunned
			if (enemy.timerStun <= 0)
				hit (moveDirection * -1, enemy.dmg);
		}
		if (coll.gameObject.CompareTag("Water")  && stepladder != null) {
			Debug.Log("Touched water! " + coll.GetContact(0).point);
			
			Vector3 hitPosition = Vector3.zero;
			foreach (ContactPoint2D hit in coll.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;

				if(GameObject.FindGameObjectsWithTag("Stepladder").Length == 0)
					Instantiate (stepladder, Vector3Int.FloorToInt(hitPosition)+new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            }
		}
	}

	void OnSouth()
	{
		if(sword != null)
		{
			if(GameObject.FindGameObjectsWithTag("Sword").Length == 0)
			{
				Instantiate (sword);
			}	
		}
	}

	void OnEast()
	{
		// Bomb
		if(bomb != null) 
		{
			if(GameObject.FindGameObjectsWithTag("Bomb").Length < 30)
				Instantiate (bomb);
		}
	}

	void OnNorth()
	{
		// Feather Jump
		if(feather != null) 
		{
			GameObject[] feathers;
			feathers = GameObject.FindGameObjectsWithTag("Feather");
			if(feathers.Length == 0)
				Instantiate (feather);
			else
			{
				feathers[0].GetComponent<JumpFeatherItem>().buttonPressed();
			}
		}
	}

	void OnWest()
	{
		// Boomerang
		if(boomerang != null) 
		{
			if(GameObject.FindGameObjectsWithTag("Boomerang").Length < 30)
				Instantiate (boomerang);
		}
	}	

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag("Enemy")) {
			EnemyMovement enemy = coll.gameObject.GetComponent<EnemyMovement> ();
			// Only does damage if the enemy is not stunned
			if (enemy.timerStun <= 0)
				hit (moveDirection * -1, enemy.dmg);
		}
	}

	public override void hit(Vector2 direction, int dmg)
	{
		base.hit(direction, dmg);
		GameManager.instance.UpdateHearts ();
	}

	public override void addHealth(int amount)
	{
		base.addHealth(amount);
		GameManager.instance.UpdateHearts ();
	}

	public void addItem(GameObject item)
	{
		//if (item.CompareTag("Sword"))
		//{
		//	sword = item;
		//}
		switch (item.tag)
      	{
          	case "Sword":
			  sword = item;
			  break;
			case "Bomb":
              bomb = item;
			  break;
          	case "Boomerang":
              boomerang = item;
			  break;
			case "HeartContainer":
			  GameManager.instance.maxHeart ++;
			  curHp += GameManager.instance.hpPerHeart;
			  GameManager.instance.UpdateHearts();
			  break;
			case "Feather":
			  feather = item;
			  break;
			case "Tunic":
			  tunic = item;
			  break;
			case "Stepladder":
			  stepladder = item;
			  break;
          	default:
              break;
      	}
	}

	IEnumerator ChangeScene(string scene)
	{
		GameObject screenTransition = GameObject.FindGameObjectWithTag("ScreenTransition");
		if (screenTransition != null)
		{
			GameManager.instance.cutscene = true;
			screenTransition.GetComponent<Animator>().SetTrigger("Start");
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene(scene);
		}
	}
}
