using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController Instance;
    public void ChangeView(GameMode next)
    {
        if(next == GameMode.MUSIC)
        {
            transform.localPosition = new Vector3( 9.6f, 0.0f, -9.37f);
        }
        if (next == GameMode.TELEPHONE)
        {
            transform.localPosition = new Vector3(-9.6f, 0.0f, -9.37f);
        }
    }
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
}
