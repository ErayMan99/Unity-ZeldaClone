using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartScene());
    }

	IEnumerator StartScene()
	{
        //this.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        GameManager.instance.cutscene = false;
	}
}
