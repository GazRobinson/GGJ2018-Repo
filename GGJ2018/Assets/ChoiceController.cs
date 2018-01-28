using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Choice
{
    ANSWER1,
    ANSWER2,
    HOLD,
    HANGUP
}
public class ChoiceController : MonoBehaviour {
    public static ChoiceController Instance = null;

    private GameObject ChoiceContainer;
    public GameObject answer1, answer2, HangUp, Hold;
    string answerA, answerB;
    private void Awake()
    {
        Instance = this;
        ChoiceContainer = transform.GetChild(1).gameObject;
        Hide();
    }


    public void ShowChoices(string _answerA, string _answerB, bool hangUp, bool hold, int[] referral)
    {
        Debug.Log("Show choices");
        answerA = _answerA;
        answerB = _answerB;
        if (answerA.Length > 0)
        {
            answer1.gameObject.SetActive(true);
            answer1.transform.GetChild(0).GetComponent<Text>().text = answerA;
    }
        else
        {
            if (referral.Length > 0 && referral[0] >0)
            {
                answer1.gameObject.SetActive(true);
                answer1.transform.GetChild(0).GetComponent<Text>().text = "...";
            }
            else
            {
            answer1.gameObject.SetActive(false);
        }
        }
        if (answerB.Length > 0)
        {
            answer2.gameObject.SetActive(true);
            answer2.transform.GetChild(0).GetComponent<Text>().text = answerB;
        }
        else
        {
            answer2.gameObject.SetActive(false);
        }
        HangUp.GetComponent<Image>().color = HangUp ? Color.white : Color.grey;
        Hold.GetComponent<Image>().color = hold ? Color.white : Color.grey;
        ChoiceContainer.SetActive(true);
    }
    public void Hide()
    {
        ChoiceContainer.SetActive(false);
    }
   public void SelectChoice(Choice newChoice)
    {
        if (ChoiceContainer.activeInHierarchy)
        {
            switch (newChoice)
            {
                case Choice.ANSWER1:
                    if (answer1.activeInHierarchy)
                    {
                        ConversationManager.Instance.AddMessage(answerA, true);
                        ConversationManager.Instance.MakeChoice(0);
                    }
                    break;
                case Choice.ANSWER2:
                    if (answer2.activeInHierarchy)
                    {
                        ConversationManager.Instance.AddMessage(answerB, true);
                        ConversationManager.Instance.MakeChoice(1);
                    }
                    break;
                case Choice.HANGUP:
                    ConversationManager.Instance.HangUp();
                    break;
                case Choice.HOLD:
                    ConversationManager.Instance.Hold();
                    break;
            }
        }
    }
    
}
