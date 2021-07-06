using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenGizmos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int x = -15; x <= 15; x++)
        {
            for (int y = -15; y <= 15; y++)
            {
                Gizmos.DrawWireCube(new Vector3((16*x)-8,(11*y)-5.5f,0), new Vector3(16, 11, 1));
            }
        }
        
    }

}
