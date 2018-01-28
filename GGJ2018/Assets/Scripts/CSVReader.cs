using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct RadioDataStrings
{
    public string ID;
    public string Question;
    public string Answer1;
    public string Answer2;
    public string OnHold;
    public string HangUp;
    public string Referral;

    public RadioDataStrings(string _ID, string _question, string _Answer1, string _Answer2, string _OnHold, string _HangUp, string _Referral)
    {
        ID = _ID;
        Question = _question;
        Answer1 = _Answer1;
        Answer2 = _Answer2;
        OnHold = _OnHold;
        HangUp = _HangUp;
        Referral = _Referral;
    }
}

[System.Serializable]
public struct RadioData
{
    public int ID;
    public string Question;
    public string[] Answers;
    public bool OnHold;
    public bool HangUp;
    public int[] Referral;

    public RadioData(int _ID, string _Question, string[] _Answers, bool _OnHold, bool _HangUp, int[] _Referral)
    {
        ID = _ID;
        Question = _Question;
        Answers = _Answers;
        OnHold = _OnHold;
        HangUp = _HangUp;
        Referral = _Referral;
    }
}

public class CSVReader : MonoBehaviour {
public static CSVReader Instance;
    public TextAsset                csvFile             = null;
    public static bool              isReady             = false;
    public bool                     overrideLanguage    = false;
    public int                      argCount            = 6;
    private int                     lineIndex           = 0;
    private string[]                lines               = null;
    
public List<RadioDataStrings> dataStrings = new List<RadioDataStrings>();
    public List<RadioData> finalData;
    public List<List<RadioDataStrings>> conversations;
    public List<List<RadioData>> convoData;

    public List<RadioDataStrings> currentstrings;
    public List<RadioData> currentData;
    private int convoIndex = 0;


    // Use this for initialization
    void Awake () {
        Instance = this;
       /* if ( overrideLanguage ) {
            Load();
        }*/
        dataStrings = StaticLoadStrings();
        ParseData(conversations);
    }
    
    public List<RadioData> LoadRandomConvo()
    {
        if (convoData.Count > 0)
        {
            convoIndex = Random.Range(0, convoData.Count);
            currentData = convoData[convoIndex];
            // currentstrings = conversations[convoIndex];
            convoData.RemoveAt(convoIndex);
        }
        else
        {
            currentData = null;
        }
        return currentData;
    }

    public static List<RadioDataStrings> StaticLoadStrings ( ) {
        TextAsset file = Instance.csvFile; // Resources.Load<TextAsset>( "CaesarTalk" );
        string[] lines = file.text.Split( "\n"[0] );
        string[] firstLine = lines[0].Split(',');
        int argCount = Instance.argCount;
        int lineIndex = 1;
        Instance.conversations = new List<List<RadioDataStrings>>();

        //List<RadioDataStrings> current = new List<RadioDataStrings>();

        List < RadioDataStrings > data = new List<RadioDataStrings>();

        while ( lineIndex < lines.Length ) {
            List<string> vars = new List<string>();
            string currentLine = lines[lineIndex];
            int loopCount = 0;

       //     Debug.Log(currentLine.Length);
            while ( vars.Count < argCount && loopCount < 1000 ) {
               // Debug.Log("LNGTH: " + currentLine.Length +", " + currentLine);
                loopCount++;

                if ( currentLine.Contains( "\"" ) ) {

                    //The line contains either a comma or a new line character
                    int quoteIndex = currentLine.IndexOf("\"");

                    //Get all complete variables before the opening quote
                    string[] preQuote = currentLine.Substring(0, quoteIndex).Split(',');
                    for ( int i = 0; i < preQuote.Length - 1; i++ ) {
                        vars.Add( preQuote[i] );
                    }

                    //Add the next line and search for the closing quote
                    bool hasQuote=currentLine.Substring( quoteIndex+1 ).Contains("\"");
                    currentLine = currentLine.Substring( quoteIndex + 1 );
                    while ( !hasQuote ) {
                        Debug.Log("Don't have a quote");
                        currentLine += "\n" + lines[++lineIndex];
                        hasQuote = currentLine.Contains( "\"" );

                    }

                    int secondQuote = currentLine.Substring(1).IndexOf('"');
                    string finalVar = "\"" + currentLine.Substring( 0, secondQuote + 2 );
                    finalVar = finalVar.Remove( 0, 1 );
                    finalVar = finalVar.Remove( finalVar.Length-1, 1 );
                    vars.Add( finalVar );
                    if (secondQuote + 3 < currentLine.Length)
                    {
                        currentLine = currentLine.Substring(secondQuote + 3);
                    }
                    //Set up the next line, minus the end of the previous variable
                 /*   if ( lineIndex < lines.Length - 1 ) {
                        
                    } else{
                        Debug.Log("FUCK");
                        currentLine = currentLine.Substring( secondQuote + 3 );
                        //lineIndex++;
					}*/

                } else {
                    string[] variables = currentLine.Split(',');
                    vars.AddRange( variables );
                }
            }
            if ( loopCount < 1000 ) {
                if (vars[0].Length < 1)
                {
                    //NEXT CONVO
                    //Debug.Log("New convo");
                   Instance.conversations.Add(data);
                    data = new List<RadioDataStrings>();
                }
                else
                {
                    if (vars.Count > 5 && vars[0].Length > 0)
                    {
                        data.Add(new RadioDataStrings(vars[0], vars[1], vars[2], vars[3], vars[4], vars[5], vars[6]));
                        Instance.dataStrings.Add(new RadioDataStrings(vars[0], vars[1], vars[2], vars[3], vars[4], vars[5], vars[6]));
                    }
                }
                lineIndex++;
            } else {
                Debug.LogError( "Inifinite loop, trying to end gracefully." );
            }
        }

        Instance.conversations.Add(data);

        return data;
    }

    public List<List<RadioData>> ParseData(List<List<RadioDataStrings>> dataString)
    {
        Instance.convoData = new List<List<RadioData>>();
        int initialID = -2;
        foreach (List<RadioDataStrings> strings in dataString)
        {
            List<RadioData> data = new List<RadioData>();
            List<string> answers = new List<string>();

            for (int i = 0; i < strings.Count; i++)
            {
                if(i == 0)
                {
                    initialID = int.Parse(strings[i].ID);
                }
                answers.Clear();
                if (strings[i].Answer1.Length > 0)
                {
                    answers.Add(strings[i].Answer1);
                }
                else
                {
                    answers.Add("");
                }
                if (strings[i].Answer2.Length > 0)
                {
                    answers.Add(strings[i].Answer2);
                }
                else
                {
                    answers.Add("");
                }
                /*  if (strings[i].OnHold.Length > 0)
                  {
                      answers.Add(strings[i].OnHold);
                  }
                  if (strings[i].HangUp.Length > 0)
                  {
                      answers.Add(strings[i].HangUp);
                  }*/
                string[] referral = strings[i].Referral.Split(',');
                int[] refInt = new int[referral.Length];
                for (int j = 0; j < referral.Length; j++)
                {
                    int parseVal = -1;
                    if (int.TryParse(referral[j], out parseVal))
                    {
                        refInt[j] = parseVal - initialID;
                    }
                    else
                    {
                        Debug.LogWarning("Failed to parse Referral");
                        refInt[j] = -2;
                    }
                }

                data.Add(new RadioData(int.Parse(strings[i].ID) - initialID,
                strings[i].Question,
                answers.ToArray(),
                strings[i].OnHold.Length < 1 || strings[i].OnHold == "ALLOWED",
                strings[i].HangUp.Length < 1 || strings[i].HangUp == "ALLOWED",
                refInt));
            }
            Instance.convoData.Add(data);
        }
    return Instance.convoData;
    }
     
}