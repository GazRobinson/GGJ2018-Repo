using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
public class MusicManager : MonoBehaviour {
    public static MusicManager Instance = null;

    string path = "";
    string MusicFolder = "/Music/";
    public AudioSource src;

    public TapeSpinner spinner = null;

    public ScriptableSong[] availableSongs;
    public int selectionIndex;
    public ScriptableSong currentSong = null;
    string[] audioFiles;
    public AudioSource horn;
    public AudioSource scratch;
    public AudioSource sfx;

    public AudioClip selectTapeSound, ejectTapeSound;

    public static void PlayClip(AudioClip clip, bool setScratch = false)
    {
        Instance.sfx.PlayOneShot(clip);
        if (setScratch)
        {
            Instance.scratch.Stop();
            Instance.scratch.clip = clip;
            Instance.scratch.Play();
        }
    }
   // public void SetScratch
    IEnumerator LoadSong(string fileName)
    {
        /*  using (var www = new WWW(MusicFolder + fileName))
          {
              yield return www;
              src.clip = www.GetAudioClip();
              src.Play();
          }*/


        /*  using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + Path.Combine(Globals.DataPath, Globals.AssistantName) + "//" + Path.GetFileName(Globals.ExpertData.AudioClipPaths[Globals.Step]), AudioType.WAV))
          {
              www.Send();
              if (www.isHttpError || www.isNetworkError)
              {
                  Debug.Log(www.error);
              }
              else
              {
                  audioClip = DownloadHandlerAudioClip.GetContent(www);
              }
          }*/

        Debug.Log(path);
      //  using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
      //  {

            UnityWebRequest www2 = UnityWebRequestMultimedia.GetAudioClip("file:///" + path + "/Ignition.ogg", AudioType.OGGVORBIS);
            yield return www2.SendWebRequest();

           // yield return www.SendWebRequest();

            if (www2.isHttpError || www2.isNetworkError)
            {
                Debug.Log(www2.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www2);
                Debug.Log("Success?: " + myClip.name);
                src.clip = myClip;
                src.Play();
            }
      //  }
    }

    private void Awake()
    {
        Instance = this;
        availableSongs = Resources.LoadAll<ScriptableSong>("Songs");
        spinner.RemoveTape();
    }

    // Use this for initialization
    void Start () {
    /*    path = Application.dataPath + "/Resources/Music";
        src = GetComponent<AudioSource>();

        var info = new DirectoryInfo(path);
        FileInfo[] fileInfo = info.GetFiles("*.ogg", SearchOption.AllDirectories);

        foreach (FileInfo file in fileInfo) print(file.Name);


        StartCoroutine(LoadSong("Ignition.ogg"));*/
	}
	public void PlayHorn()
    {
        horn.Stop();
        horn.Play();
    }
    public void SelectCurrent()
    {
        currentSong = availableSongs[selectionIndex];

        src.clip = currentSong.audioFile;
        Debug.Log("Equipped " + src.clip.name);
        PlayClip(selectTapeSound);
        spinner.SetTape(CassetteShelf.Instance.GetTex(selectionIndex));
        src.Play();
    }
    public void StopSong()
    {
        src.Stop();
        src.clip = null;
        currentSong = null;
        spinner.RemoveTape();
        PlayClip(ejectTapeSound);
    }
    public float scratchFactor = 25f;
    public void Scratch(float spin, bool onTrack = false)
    {
        if (onTrack)
        {
            src.pitch = spin * 10f;
            spinner.RPM = 7500f * spin;
        }
        else
        {
            scratch.pitch = spin * scratchFactor;
        }
    }
    public void Unscratch()
    {
        src.pitch = 1f;
        spinner.RPM = 100f;
        scratch.pitch = 0f;
    }

    public void LoadSong(ScriptableSong newSong)
    {

    }

    public void ChangeSelection(int increment)
    {
        selectionIndex += increment;
        if(selectionIndex < 0)
        {
            selectionIndex = availableSongs.Length - 1;
        }
        if(selectionIndex >= availableSongs.Length)
        {
            selectionIndex = 0;
        }
        Debug.Log(availableSongs[ selectionIndex].songTitle);
        CassetteShelf.Instance.SetTape(selectionIndex);
    }

    public void SpinUp()
    {
       /* if(currentSong != null && src.clip != null && !src.isPlaying)
        {
            src.Play();
        }*/
    }
}
