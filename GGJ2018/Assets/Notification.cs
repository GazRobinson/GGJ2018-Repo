using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour {
    private float speed = 20.0f;

	// Update is called once per frame
	void Update () {
        float t = Mathf.Sin(Time.time* speed);
        t = Mathf.Round(t);
        float angle = 30f * t;
        transform.localEulerAngles = new Vector3(0f, 0f, angle);
	}
}
