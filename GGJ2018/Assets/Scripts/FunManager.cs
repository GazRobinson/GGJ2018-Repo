using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunManager : MonoBehaviour {

    public static FunManager    Instance = null;
    public float                FunLevel = 0f;

    public float                displayFun = 0f;
    private float               lastFun = 0f;

    public Text                 funDisplay;

    float                       t = 0f;

	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
    public static void AddFun(float funAdd) {
        Instance.lastFun = Instance.displayFun;
        Instance.FunLevel += funAdd;
        Instance.t = 0f;
    }

	// Update is called once per frame
	void Update () {
        AddFun( -0.1f * Time.deltaTime );
        if (t < 1f) {
            t += Time.deltaTime;
            t = Mathf.Clamp01(t);
            displayFun = Mathf.Lerp(displayFun, FunLevel, Time.deltaTime);
        }
        funDisplay.text = "FUN: " + displayFun.ToString();
	}
}
