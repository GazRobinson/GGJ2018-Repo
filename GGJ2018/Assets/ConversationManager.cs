using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour {
    public static ConversationManager Instance;

    public GameObject usrMsg, playerMsg;

    public float margin = 32f;

    private RectTransform window;

    private List<RectTransform> msgList = new List<RectTransform>();
    public ScrollRect scrollRect;

    float scrollY = 0f;
    int currentStep = 0;

    public List<RadioData> currentConversation;

    public void AddMessage(string message, bool player)
    {
        GameObject newMessage = GameObject.Instantiate(player? playerMsg : usrMsg, window, false);
        msgList.Add(newMessage.GetComponent<RectTransform>());
        newMessage.transform.GetChild(0).GetComponent<Text>().text = message.ToUpper();
        RectTransform rectT = newMessage.GetComponent<RectTransform>();
        Vector2 size = rectT.sizeDelta;
        size.y = newMessage.transform.GetChild(0).GetComponent<Text>().preferredHeight + 100f;
        rectT.sizeDelta = size;
        Sort();
    }
    
    public void StartConvo(List<RadioData> newConvo)
    {
        currentConversation = newConvo;
        currentStep = 0;
        DoConvoStep(currentStep);
    }
    public void MakeChoice(int answer)
    {
        int referral = currentConversation[currentStep].Referral[answer];
        if (referral > 0) {
            currentStep = referral;
            DoConvoStep(currentStep);
        }
        else
        {
            //END CONVO
            AddMessage("They hung up", false); Cleanup();
        }
    }
    public void HangUp()
    {
        currentConversation = null;
        currentStep = 0;
        AddMessage("You hung up", true);
        Cleanup();
    }
    void Cleanup()
    {
        ChoiceController.Instance.Hide();
        SwitchboardController.Instance.HangUp();
        
    }
    public void Hold()
    {

    }
    void DoConvoStep(int step)
    {
        AddMessage(currentConversation[step].Question, false);
        
        ChoiceController.Instance.ShowChoices(currentConversation[step].Answers[0], currentConversation[step].Answers[1], currentConversation[step].HangUp, currentConversation[step].OnHold, currentConversation[step].Referral);
    }

    private void Sort()
    {
        float yVal = 0f;
        for(int i =0; i < msgList.Count; i++)
        {
            Vector2 pos = msgList[i].anchoredPosition;
            pos.y = yVal;
            msgList[i].anchoredPosition = pos;

            yVal -= msgList[i].sizeDelta.y;
            yVal -= margin;
        }
        Vector2 size = window.sizeDelta;
        size.y = -yVal;
        window.sizeDelta = size;

        float diff = -yVal - 1080f;

        scrollRect.verticalNormalizedPosition = 0;
    }

    [ContextMenu("MessageTest")]
    public void MessageTest()
    {
        AddMessage("Then i shall reserve my desire for the next player standing behind you, who will play as the DJ when you inevitable fail and game over... good day", false);
    }
    [ContextMenu("Player MessageTest")]
    public void PlyrMessageTest()
    {
        AddMessage("Then i shall reserve my desire for the next player standing behind you, who will play as the DJ when you inevitable fail and game over... good day", true);
    }
    // Use this for initialization
    void Awake () {
        Instance = this;
        window = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>();
	}

    private void Start()
    {
       /* GetConvo();
        StartConvo();*/
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Random.value < 0.5f)
            {
                MessageTest();
            }
            else
            {
                PlyrMessageTest();
            }
        }
	}
}
