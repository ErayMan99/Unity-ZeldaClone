using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeever : EnemyMovement
{
    public bool isUnderground = true;
    private bool switchState = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start ();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update ();
        if (isUnderground)
        {
            gameObject.layer = 10;
            if (!switchState)
            {
                switchState = true;
                Invoke ("emerge", Random.Range (1f, 3f));
            }
        }
        else
        {
            gameObject.layer = 9;
            if (!switchState)
            {
                switchState = true;
                Invoke ("burrow", Random.Range (2f, 4f));
            }
        }
    }
    
    public override void FixedUpdate()
	{
		base.FixedUpdate ();
	}

    public void OnCollisionEnter2D(Collision2D coll)
	{
		// If we hit an obstacle, find another direction
		if (coll.gameObject.tag == "Wall") {
			timerMovement = 0;
		}
	}	
    public override void hit(Vector2 direction, int dmg)
	{
        if(animator.GetBool("Emerging"))
            base.hit (direction, dmg);
	}

    void emerge()
    {
        animator.SetBool("Emerging", true);
        switchState = false;
    }

    void burrow()
    {
        animator.SetBool("Emerging", false);
        switchState = false;
    }
}
