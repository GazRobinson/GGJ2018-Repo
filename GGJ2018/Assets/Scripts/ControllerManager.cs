using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    NONE,
    MUSIC,
    TELEPHONE
}
public class KeyboardManager{
    public static KeyCode[] alphaKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
    public static KeyCode[] numPadKeys = { KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0 };
    public static KeyCode[] topRow = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P};
}
public class ControllerManager : MonoBehaviour {
    public static ControllerManager Instance = null;
    public GameMode currentGameMode;
    
    public float knobThreshold = 0.2f;
    public AudioClip crossfadeSound;
    public GameObject controls;

    public bool is_a_DJ = false;
    public bool has_numpad = false;

    private float lastKnob = 0f;
    bool scratch = false;

    public Vector2 lastMouse;

    public float ScratchAmount{
        get{
            return is_a_DJ ? Input.GetAxisRaw("Vertical") : Input.mouseScrollDelta.y * 0.01f;
        }
    }

	// Use this for initialization
	void Awake () {
        Instance = this;
        lastKnob = Input.GetAxisRaw("Knob");
        lastMouse = Input.mousePosition;
    }
	
    void Start(){
        ChangeMode(-1f);
    }

	// Update is called once per frame
	void Update () {
        if (is_a_DJ)
        {
            ChangeMode(Input.GetAxisRaw("Crossfade"));
        }
        else{
            if(Input.GetKeyDown(KeyCode.Tab)){
                if (currentGameMode == GameMode.MUSIC)
                    ChangeMode(1f);
                else
                    ChangeMode(-1f);
            }
        }

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

        MusicManager.Instance.Scratch(ScratchAmount, scratch);
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
            if (Mathf.Abs(ScratchAmount) > 0.1f)
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
            FunManager.AddFun(10.0f);
            MusicManager.Instance.PlayHorn();
        }
        if (is_a_DJ)
        {
            if (Mathf.Abs(dist) > knobThreshold)
            {
                if (dist > 1f)
                {
                    float t = -1 - lastKnob;
                    float t2 = 1 - thisK;
                    dist = t - t2;
                    //  Debug.Log("Tick down");
                }
                else if (dist < -1f)
                {
                    float t = 1 - lastKnob;
                    float t2 = -1 - thisK;
                    dist = t - t2;
                }

                if (dist > 0)
                {
                    MusicManager.Instance.ChangeSelection(1);
                }
                else
                {
                    MusicManager.Instance.ChangeSelection(-1);
                }
                lastKnob = Input.GetAxisRaw("Knob");
            }
        } else{
            if (Input.GetKeyDown(KeyCode.UpArrow))
                MusicManager.Instance.ChangeSelection(-1);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                MusicManager.Instance.ChangeSelection(1);
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
