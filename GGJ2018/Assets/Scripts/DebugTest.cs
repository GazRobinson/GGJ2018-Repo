using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class DebugTest : MonoBehaviour {
    public bool debugOut = false;
    GamePadState state;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (debugOut)
        {
            if (Input.GetButton("A"))
            {
                Debug.Log("A");
            }
            if (Input.GetButton("B"))
            {
                Debug.Log("B");
            }
            if (Input.GetButton("X"))
            {
                Debug.Log("X");
            }
            if (Input.GetButton("Y"))
            {
                Debug.Log("Y");
            }
            Debug.Log("spin " + Input.GetAxisRaw("Vertical"));
        }
        float CF = Input.GetAxisRaw("Crossfade");
        if (Mathf.Abs(CF) > 0f) {
         //   Debug.Log("CFade " + CF);
        }
      //  Debug.Log("Knob " + Input.GetAxisRaw("Knob"));
        GamePad.SetVibration((PlayerIndex)0, 0f, 0.5f*(Mathf.Sin(Time.time*4f)+1));
    }
}
