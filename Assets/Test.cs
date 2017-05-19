using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public  Slider process;
	// Use this for initialization
	void Start ()
    {	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( process )
        {
            process.value += 0.01f;
            if( process.value >= 1f )
            {
                process.value = 0;
            }
        }
	}
}
