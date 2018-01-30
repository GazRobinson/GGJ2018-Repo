using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeSpinner : MonoBehaviour {
    public Renderer tapeRender;
    public float RPM
    {
        get { return rpm;}
        set { rpm = value; }
    }
    float rpm = 100f;
	// Use this for initialization
	void Start () {
		
	}

    public void SetTape(Texture tex)
    {
        tapeRender.material.mainTexture = tex;
        tapeRender.gameObject.SetActive(true);
    }
    public void RemoveTape()
    {
        tapeRender.gameObject.SetActive(false);
    }

	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, rpm * Time.deltaTime);
	}
}
