using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Funometer : MonoBehaviour {
    public float maximumFun =  10f;
    public float minimumFun = -10f;

    private float currentFun = 0f;
    private RectTransform arrowTransform = null;
	// Use this for initialization
	void Start () {
        arrowTransform = transform.GetChild(0).GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        currentFun = FunManager.Instance.displayFun;
        UpdateArrow();
	}

    private void UpdateArrow(){
        arrowTransform.localEulerAngles = new Vector3(0f, 0f, (Mathf.InverseLerp(minimumFun, maximumFun, currentFun) - 0.5f) * -180f);
    }
}
