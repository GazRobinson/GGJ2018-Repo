using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchboardController : MonoBehaviour {
    public static SwitchboardController Instance;
    public SwitchboardButton[] buttons = new SwitchboardButton[9];
    public float MaxWaitTime = 5f;

    float nextCall = 0f;
	// Use this for initialization
	void Awake () {
        Instance = this;
        GetButtons();
        nextCall = Time.time + 5f;
	}

    void GetButtons()
    {
        buttons = new SwitchboardButton[9];
        for (int i=0; i < 9; i++)
        {
            buttons[i] = transform.GetChild(i).GetComponent<SwitchboardButton>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextCall)
        {
            GetCall();

            nextCall = Time.time + 5f;
        }
        DoInput();
    }

    private void DoInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            AnswerCall(1);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            AnswerCall(2);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            AnswerCall(3);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            AnswerCall(4);
        if (Input.GetKeyDown(KeyCode.Keypad5))
            AnswerCall(5);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            AnswerCall(6);
        if (Input.GetKeyDown(KeyCode.Keypad7))
            AnswerCall(7);
        if (Input.GetKeyDown(KeyCode.Keypad8))
            AnswerCall(8);
        if (Input.GetKeyDown(KeyCode.Keypad9))
            AnswerCall(9);
    }
    int currentCall = -1;
    private void AnswerCall(int callNumber)
    {
        currentCall = callNumber - 1;
        buttons[callNumber - 1].Answer();
    }

    public void HangUp()
    {
        buttons[currentCall].HangUp();
        currentCall = -1;
    }

    public void GetCall()
    {
        int free = GetFreeButton();
        if (free < 0)
        {
            Debug.Log("GAME OVER");
            return;
        }
        else
        {
            List<RadioData> convo = CSVReader.Instance.LoadRandomConvo();
            if (convo != null)
            {
                buttons[free].Activate(convo);
            }
            else
            {
                Debug.Log("No more calls");
            }
        }
    }

    public int GetFreeButton()
    {
        int[] rnd = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        

        for (int i=0; i < 100; i++)
        {
            int a = Random.Range(0, 9);
            int b = Random.Range(0, 9);
            int tmp = rnd[a];
            rnd[a] = rnd[b];
            rnd[b] = tmp;
        }
        for(int index = 0; index < rnd.Length; index++)
        {
            if(buttons[rnd[index]].state == CallState.NONE)
            {
                return rnd[index];
            }
        }

        return -1;
    }
}
