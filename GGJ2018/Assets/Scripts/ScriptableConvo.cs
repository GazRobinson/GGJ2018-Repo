using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Convo", menuName = "Conversation", order = 1)]
public class ScriptableConvo : ScriptableObject {

    public bool OnHold;
    public bool HangUp;

    public ConversationStep baseStep;

    public ConversationStep[] additionalSteps;

}

[System.Serializable]
public struct ConversationStep
{
    public string Dialogue;
    public Answer[] Answers;

}

[System.Serializable]
public struct Answer
{
    public string AnswerText;
    public int Next;
}
