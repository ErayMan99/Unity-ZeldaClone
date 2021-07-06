using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;     //Allows other scripts to call functions from SoundManager.             

	public bool cutscene;
	private int rupeeTotal;
	private int rupeeShown;
	private Text rupeeText;
	[SerializeField]
	public Vector2 screenSize;
	private Vector2 currentScreen;
	private Vector2 previousScreen;
	private GameObject[] listEnemies;
	private GameObject player;
	private PlayerMovement playerMovement;
	public Image[] heartsUI;
	public Sprite[] heartsSprites;
	public float maxHeart;
	public int hpPerHeart;
	public AudioClip audioTreasure;
	
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

	public void Initialize()
	{
		rupeeText = GameObject.FindGameObjectWithTag ("RupeeAmount").GetComponent<Text>();
		listEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMovement = player.GetComponent<PlayerMovement> ();
		UpdateHearts ();
		changeScreen (new Vector2(0,0));
	}

	void FixedUpdate()
	{
		if (rupeeShown < rupeeTotal)
			rupeeShown++;
		if (rupeeShown > rupeeTotal)
			rupeeShown--;

		rupeeText.text = rupeeShown.ToString("000");
	}

	public Vector2 getVectorFromDirection(float direction)
	{
		switch ((int)direction) {
		case 0:
			return Vector2.up;
			break;
		case 1:
			return Vector2.left;
			break;
		case 2:
			return Vector2.down;
			break;
		case 3:
			return Vector2.right;
			break;
		default:
			return Vector2.zero;
		}
		return Vector2.zero;
	}

	public float getDirectionFromVector(Vector2 direction)
	{
		if (direction.x == -1)
			return 1f;
		if (direction.y == 1)
			return 0f;
		if (direction.x == 1)
			return 3f;
		if (direction.y == -1)
			return 2f;

		return 0f;
	}

	public void addRupee(int amount)
	{
		rupeeTotal += amount;
	}

	// Returns screen position based on a vector2
	public Vector2 getScreenFromPosition(Vector2 position)
	{
		return new Vector2 (Mathf.Floor(position.x/screenSize.x), Mathf.Floor(position.y/screenSize.y));
	}

	// log previous screens and activate/deactivate proper enemies
	public void changeScreen(Vector2 newScreen)
	{
		// deactivate enemies / objects of previous screen, except if same screen as current
		foreach (var Enemy in listEnemies) {
			Vector2 enemyScreen = getScreenFromPosition (Enemy.transform.position);

			// If in new screen and not in the last 2 screens, activate
			if (enemyScreen == newScreen) {
				// If in new screen and also a previous screen, do nothing
				if (newScreen != previousScreen)
					Enemy.SetActive (true);
			} else if (enemyScreen == currentScreen) {
				// do nothing
			}
			else
				Enemy.SetActive (false);
		}
			
		// change previous screens
		previousScreen = currentScreen;
		currentScreen = newScreen;
	}

	public bool isCurrentScreen(Vector2 position)
	{
		return (currentScreen == getScreenFromPosition(position));
	}

	public void UpdateHearts()
	{
		float curHp = playerMovement.curHp;

		for (int i = 0; i < heartsUI.Length; i++)
		{
			if (maxHeart > i)
			{
				heartsUI [i].enabled = true;
				// Full heart
				if (Mathf.FloorToInt(Mathf.Ceil(curHp)/(float)hpPerHeart) > i) {
					heartsUI [i].sprite = heartsSprites [2];
				} 
				// half heart
				else if (Mathf.Ceil(curHp) == (float)((i + 1) * hpPerHeart) - (hpPerHeart / 2)) 
				{
					heartsUI [i].sprite = heartsSprites [1];
				}
				// empty heart
				else 
					heartsUI [i].sprite = heartsSprites [0];

				// Current heart is made bigger in the UI
				if (Mathf.CeilToInt ((float)curHp / 2) == i+1) {
					heartsUI [i].rectTransform.sizeDelta = new Vector2 (40, 40);
				} else {
					heartsUI [i].rectTransform.sizeDelta = new Vector2 (32, 32);
				}
			}
			else
			{
				heartsUI [i].enabled = false;
			}
		}
	}

	public void getTreasure(GameObject treasure)
	{
		cutscene = true;
		treasure.transform.position = player.transform.position + Vector3.up*1.5f + Vector3.right/2;
		SoundManager.instance.PlaySingle (audioTreasure);
		playerMovement.animator.SetBool ("GetTreasure", true);
		Invoke ("stopTreasure", 2f);
	}
	public void stopTreasure()
	{
		cutscene = false;
		playerMovement.animator.SetBool ("GetTreasure", false);
	}
		
}

