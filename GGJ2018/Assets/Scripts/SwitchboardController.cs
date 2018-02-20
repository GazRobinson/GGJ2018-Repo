using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchboardController : MonoBehaviour {
    public static SwitchboardController Instance;
    public SwitchboardButton[] buttons = new SwitchboardButton[9];
    public float MinTimeBetweenCalls = 5f;
    public float MaxTimeBetweenCalls = 15f;
    public float maxmimumPatienceTime = 20.0f;

    public AudioClip pickupSound, endCallSound, holdSound;

    float nextCall = 0f;
    private KeyCode[] keys;
	// Use this for initialization
	void Awake () {
        Instance = this;
        GetButtons();
        nextCall = Time.time + 5f;
	}
    void Start(){
        keys = ControllerManager.Instance.has_numpad ? KeyboardManager.numPadKeys : KeyboardManager.alphaKeys;
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

            nextCall = Time.time + Random.Range(MinTimeBetweenCalls, MaxTimeBetweenCalls);
        }
        DoInput();
    }

    private void DoInput()
    {
        for (int i = 0; i < 9; i++){
            if (Input.GetKeyDown(keys[i]))
                AnswerCall(i+1);
        }

    }
    int currentCall = -1;
    private void AnswerCall(int callNumber)
    {
        MusicManager.PlayClip(pickupSound);
        currentCall = callNumber - 1;
        buttons[callNumber - 1].Answer();
    }

    public void HangUp()
    {
        MusicManager.PlayClip(endCallSound);
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
