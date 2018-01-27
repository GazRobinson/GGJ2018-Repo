using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSong", menuName = "Song", order = 1)]
public class ScriptableSong : ScriptableObject {

    public AudioClip audioFile;
    public string artist;
    public string songTitle;
}
