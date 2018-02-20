using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;

//docs.google.com/feeds/download/spreadsheets/Export?key<FILE_ID>&exportFormat=csv&gid=0

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


    IEnumerator DownloadLatest()
    {
        //https://docs.google.com/spreadsheets/d/1eNnksFIX2YKiBCWnWxTMPSQe8xmOis5wpwUndE5oavw/edit?usp=sharing
        UnityWebRequest www = UnityWebRequest.Get("http://docs.google.com/feeds/download/spreadsheets/Export?key=1eNnksFIX2YKiBCWnWxTMPSQe8xmOis5wpwUndE5oavw&exportFormat=csv&gid=0");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Download succeeded");
            downloadSuccess = true;
            fullFileText = www.downloadHandler.text;
        }
    }
    string fullFileText = "";
    bool downloadSuccess = false;
    // Use this for initialization
    void Awake () {
        Instance = this;
       /* if ( overrideLanguage ) {
            Load();
        }*/
    }
    private IEnumerator Start()
    {
        yield return StartCoroutine(DownloadLatest());

        if(!downloadSuccess){
            fullFileText = csvFile.text;
        }
        string[,] stringArray = SplitCsvGrid(fullFileText);
        BuildData(stringArray);
    }
    public List<RadioData> LoadRandomConvo()
    {
        if (convoData.Count > 0)
        {
            convoIndex = Random.Range(0, convoData.Count);
            currentData = convoData[convoIndex];
            convoData.RemoveAt(convoIndex);
        }
        else
        {
            currentData = null;
        }
        return currentData;
    }

    // splits a CSV file into a 2D string array
    static public string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);
        // finds the max width of row
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);

        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[width, lines.Length];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];

                // This line was to replace "" with " in my output. 
                // Include or edit it as you wish.
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }

    // splits a CSV row 
    static public string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }

    void BuildData(string [,] txt){
        Instance.convoData = new List<List<RadioData>>();
        int initialID = int.Parse(txt[0, 1]);
        List<RadioData> thisConvo = new List<RadioData>();

        for (int y = 1; y < txt.GetLength(1); y++){
            if(txt[0,y].Length < 1){
                //New convo
                Instance.convoData.Add(thisConvo);
                thisConvo = new List<RadioData>();
                initialID = int.Parse(txt[0, y+1]);
                continue;
            }
            int id = int.Parse(txt[0, y]);

            string question = txt[1, y];
            string[] answer = new string[]{ txt[2, y], txt[3, y] };
            bool onhold = txt[4, y] == "ALLOWED";
            bool hangup = txt[5, y] == "ALLOWED";
            int[] referral = new int[] { -2, -2 };
            Debug.Log(txt[6, y]);
            string[] refStr = txt[6, y].Split(',');

            Debug.Log("line: " + y + " is good");
            for (int i = 0; i < refStr.Length; i++){
                int reff = -3;
                int.TryParse(refStr[i], out reff);
                reff -= initialID;
                referral[i] = reff;
            }

            thisConvo.Add(new RadioData(id,question,answer,onhold,hangup,referral));
        }
        Instance.convoData.Add(thisConvo);
    }

     
}