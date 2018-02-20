using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour {
    public static Notification Instance;
    private float speed = 20.0f;
    private RectTransform rectTransform;
    void Awake(){
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public static void ShowNotification(Vector2 viewportPosition, float time){
       // Instance.SetNotification(viewportPosition, time);
    }
    void SetNotification(Vector2 viewportPosition, float time)
    {
        StopCoroutine("WaitToDie");
        rectTransform.anchoredPosition = new Vector2(viewportPosition.x * 1920f - 52f, viewportPosition.y * 1080f + 52f);
        gameObject.SetActive(true);
        StartCoroutine(WaitToDie(time));
    }
	// Update is called once per frame
	void Update () {
        float t = Mathf.Sin(Time.time* speed);
        t = Mathf.Round(t);
        float angle = 30f * t;
        transform.localEulerAngles = new Vector3(0f, 0f, angle);

        if(Input.GetMouseButtonDown(0)){
            ShowNotification(Camera.main.ScreenToViewportPoint(Input.mousePosition), 3f);
        }
	}

    IEnumerator WaitToDie(float timeUntilDeath){
        yield return new WaitForSeconds(timeUntilDeath);
        gameObject.SetActive(false);
    }
}
