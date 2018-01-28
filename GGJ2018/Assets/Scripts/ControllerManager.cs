using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    NONE,
    MUSIC,
    TELEPHONE
}

public class ControllerManager : MonoBehaviour {

    public GameMode currentGameMode;
    
    public float knobThreshold = 0.2f;
    private float lastKnob = 0f;
    bool scratch = false;
	// Use this for initialization
	void Start () {
        lastKnob = Input.GetAxisRaw("Knob");

    }
	
	// Update is called once per frame
	void Update () {
        ChangeMode(Input.GetAxisRaw("Crossfade"));

        UpdateMode();
	}

    private void UpdateMode()
    {
        switch (currentGameMode)
        {
            case GameMode.NONE:
                break;
            case GameMode.MUSIC:
                MusicUpdate();
                break;
            case GameMode.TELEPHONE:
                TelephoneUpdate();
                break;
            default:
                break;
        }
    }
    //TODO: fix this, it's really bad
    private void MusicUpdate()
    {
        float thisK = Input.GetAxisRaw("Knob");
        float dist = thisK - lastKnob;
        scratch = Input.GetButton("X");
        if (scratch)
        {
            MusicManager.Instance.Scratch(Input.GetAxisRaw("Vertical"));            
        }
        else
        {
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
            {
                MusicManager.Instance.SpinUp();
            }
        }

        if (Input.GetButtonUp("X"))
        {
            scratch = false;
            MusicManager.Instance.Unscratch();
        }


        if (Input.GetButtonDown("Y"))
        {
            MusicManager.Instance.PlayHorn();
        }

        if (Mathf.Abs(dist) > 0.4f)
        {
            if(dist > 1f)
            {
                float t = -1 - lastKnob;
                float t2 = 1 - thisK;
                dist = t - t2;
              //  Debug.Log("Tick down");
            }
            else if(dist < -1f)
            {
                float t = 1 - lastKnob;
                float t2 = -1 - thisK;
                dist = t - t2;
            }

            if(dist > 0)
            {
                MusicManager.Instance.ChangeSelection(1);
            }
            else
            {
                MusicManager.Instance.ChangeSelection(-1);
            }
            lastKnob = Input.GetAxisRaw("Knob");
        }

        if (Input.GetButtonDown("A") && MusicManager.Instance.currentSong == null)
        {
            MusicManager.Instance.SelectCurrent();
        }
        if (Input.GetButtonDown("B") )
        {
            MusicManager.Instance.StopSong();
        }
    }

    private void TelephoneUpdate()
    {
        if (Input.GetButtonDown("A"))
            ChoiceController.Instance.SelectChoice(Choice.ANSWER2);
        if (Input.GetButtonDown("Y"))
            ChoiceController.Instance.SelectChoice(Choice.ANSWER1);
        if (Input.GetButtonDown("B"))
            ChoiceController.Instance.SelectChoice(Choice.HANGUP);
        if (Input.GetButtonDown("X"))
            ChoiceController.Instance.SelectChoice(Choice.HOLD);
    }

    private void ChangeMode(float cfVal)
    {
        GameMode next = GameMode.NONE;
        if (cfVal == 0f)
        {
            next = GameMode.NONE;
        }
        else {
            next = cfVal < 0 ? GameMode.MUSIC : GameMode.TELEPHONE;
        }

        if(next != currentGameMode)
        {
            ExitCurrentMode();

            currentGameMode = next;

            EnterMode();
        }
    }
    private void EnterMode()
    {
        CameraController.Instance.ChangeView(currentGameMode);
        switch (currentGameMode)
        {
            case GameMode.NONE:
                break;
            case GameMode.MUSIC:
                break;
            case GameMode.TELEPHONE:
                break;
            default:
                break;
        }
    }
    private void ExitCurrentMode()
    {

    }
}
