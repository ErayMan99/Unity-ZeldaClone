using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTektite : EnemyMovement {

	private int timerJump;
	public bool jump = false;
	private Vector2 tempLocation;
	private float t;
	private float tempHeight;
	public float jumpHeight = 2;

	[SerializeField]
	private Vector2 nextLocation;
	private Vector2 startLocation;

	// Use this for initialization
	public override void Start () {
		base.Start ();
	}

	public override void OnEnable()
	{
		base.OnEnable ();
		timerJump = Random.Range (10, 200);
	}

	public override void Update () {
		if (isAbleToMove ()) {
			if (!jump) {
				// Jump!
				if (timerJump <= 0) {
					jump = true;
					animator.SetBool ("Jump", true);
					// Find next coord
					bool foundNextLocation = false;
					while (!foundNextLocation) {
						tempLocation = transform.position + (Vector3)(Random.insideUnitCircle * speed);
						// Make sure coord are in current screen
						if (GameManager.instance.isCurrentScreen (tempLocation)) {
							foundNextLocation = true;
							nextLocation = tempLocation;
							startLocation = (Vector2) transform.position;
						}
					}
				}
			}
		}
	}

	public override void FixedUpdate()
	{
		rigidbody.velocity = Vector2.zero;
		if (timerJump > 0)
			timerJump--;
		if (jump && curHp > 0) {
			t += Time.fixedDeltaTime;
			t = Mathf.Clamp (t, 0, 1);
			if (t < 0.5)
				tempHeight += Time.fixedDeltaTime*2;
			else
				tempHeight -= Time.fixedDeltaTime*2;

			transform.position = Vector3.Slerp (startLocation, (Vector3)nextLocation, t);
			float temp2 = ((0.5f-t) * jumpHeight)*2;
			float temp = Mathf.Sqrt((jumpHeight * jumpHeight) - (temp2*temp2));
			transform.position += new Vector3 (0, (float)temp, 0);

			if(0.01f >= Mathf.Abs(Vector2.Distance(transform.position, nextLocation))){
				jump = false;
				animator.SetBool ("Jump", false);
				timerJump = Random.Range (10, 200);
				t = 0;
				tempHeight = 0;
			}
		}

		updateTimers ();
		updateInvincibility ();
	}
}
