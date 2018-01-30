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
    public AudioClip crossfadeSound;
    public GameObject controls;
    private float lastKnob = 0f;
    bool scratch = false;
	// Use this for initialization
	void Start () {
        lastKnob = Input.GetAxisRaw("Knob");

    }
	
	// Update is called once per frame
	void Update () {
        ChangeMode(Input.GetAxisRaw("Crossfade"));

        if (Input.GetButtonDown("Start"))
        {
            controls.SetActive(!controls.activeSelf);
        }

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

        MusicManager.Instance.Scratch(Input.GetAxisRaw("Vertical"), scratch);
    }
    //TODO: fix this, it's really bad
    private void MusicUpdate()
    {
        float thisK = Input.GetAxisRaw("Knob");
        float dist = thisK - lastKnob;
        scratch = Input.GetButton("X");

        if (scratch)
        {          
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


        FunManager.AddFun(Time.deltaTime);
        if (Input.GetButtonDown("Y"))
        {
            FunManager.AddFun(4.7f);
            MusicManager.Instance.PlayHorn();
        }

        if (Mathf.Abs(dist) > knobThreshold)
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
        if (Input.GetButtonDown("B") && !Input.GetButton("X"))
        {
            MusicManager.Instance.StopSong();
        }
    }

    private void TelephoneUpdate()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            ChoiceController.Instance.SelectChoice(Choice.ANSWER2);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ChoiceController.Instance.SelectChoice(Choice.ANSWER1);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ChoiceController.Instance.SelectChoice(Choice.HANGUP);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ChoiceController.Instance.SelectChoice(Choice.HOLD);
    }

    private void ChangeMode(float cfVal)
    {
        GameMode next = currentGameMode;
        if (cfVal == 0f)
        {
           // next = GameMode.NONE;
        }
        else {
            next = cfVal < 0 ? GameMode.MUSIC : GameMode.TELEPHONE;
        }

        if(next != currentGameMode)
        {
            MusicManager.PlayClip(crossfadeSound);
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
        switch (currentGameMode)
        {
            case GameMode.NONE:
                break;
            case GameMode.MUSIC:
                MusicManager.Instance.Unscratch();
                break;
            case GameMode.TELEPHONE:
                break;
            default:
                break;
        }

    }
}
