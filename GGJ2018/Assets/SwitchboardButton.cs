using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CallState
{
    NONE,
    NORMAL,
    WARNING,
    HOLD,
    ANSWERED
}

public class SwitchboardButton : MonoBehaviour {

    public CallState state = CallState.NONE;
    public List<RadioData> conversation = null;
    public Sprite normal, warning, hold;
    private SpriteRenderer spriteRenderer;
    private float startTime = 0f;

    public float WaitTime
    {
        get
        {
            return Time.time - startTime;
        }
    }
    public void Answer()
    {
        if (state > CallState.NONE)
        {
            state = CallState.ANSWERED;
            spriteRenderer.sprite = normal;
            spriteRenderer.color = new Color(114f / 255f, 1f, 197f / 255f, 1f);

            ConversationManager.Instance.StartConvo(conversation);
        }
    }
    public void Hold()
    {
        state = CallState.HOLD;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        spriteRenderer.sprite = hold;
    }
    public void Activate(List<RadioData> newCall)
    {
        conversation = newCall;
        spriteRenderer.sprite = normal;
        state = CallState.NORMAL;
        startTime = Time.time;
    }
    public void HangUp()
    {
        conversation = null;
        spriteRenderer.sprite = null;
        state = CallState.NONE;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
        LoadImages();
    }

    void LoadImages()
    {
        normal = Resources.Load<Sprite>("Art/SwitchButtons/White_-" + (transform.GetSiblingIndex() + 1).ToString());
        warning = Resources.Load<Sprite>("Art/SwitchButtons/Red_Red-" + (transform.GetSiblingIndex() + 1).ToString());
        hold = Resources.Load<Sprite>("Art/SwitchButtons/Yellow_-" + (transform.GetSiblingIndex() + 1).ToString());
    }

    private void Update()
    {
        if(state > CallState.NONE && state < CallState.ANSWERED &&  WaitTime > 0.75f * SwitchboardController.Instance.maxmimumPatienceTime)
        {
            spriteRenderer.sprite = warning;
        }
    }

}
