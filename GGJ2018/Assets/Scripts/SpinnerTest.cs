using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

public class SpinnerTest : MonoBehaviour {
    public float modifier = 10000f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward,-Input.GetAxis("Vertical") * modifier);
	}
}
