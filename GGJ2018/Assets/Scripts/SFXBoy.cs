using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXBoy : MonoBehaviour {
    public AudioClip[] sfx;

    private KeyCode[] keys;

    void Start(){
        if(ControllerManager.Instance.has_numpad){
            keys = KeyboardManager.alphaKeys;
        } else{
            keys = KeyboardManager.topRow;
        }
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keys[0]) && sfx.Length > 0)
            MusicManager.PlayClip(sfx[0], true);
        if (Input.GetKeyDown(keys[1]) && sfx.Length > 1)
            MusicManager.PlayClip(sfx[1], true);
        if (Input.GetKeyDown(keys[2]) && sfx.Length > 2)
            MusicManager.PlayClip(sfx[2], true);
        if (Input.GetKeyDown(keys[3]) && sfx.Length > 3)
            MusicManager.PlayClip(sfx[3], true);
        if (Input.GetKeyDown(keys[4]) && sfx.Length > 4)
            MusicManager.PlayClip(sfx[4], true);
        if (Input.GetKeyDown(keys[5]) && sfx.Length > 5)
            MusicManager.PlayClip(sfx[5], true);
        if (Input.GetKeyDown(keys[6]) && sfx.Length > 6)
            MusicManager.PlayClip(sfx[6], true);
        if (Input.GetKeyDown(keys[7]) && sfx.Length > 7)
            MusicManager.PlayClip(sfx[7], true);
        if (Input.GetKeyDown(keys[8]) && sfx.Length > 8)
            MusicManager.PlayClip(sfx[8], true);
        if (Input.GetKeyDown(keys[9]) && sfx.Length > 9)
            MusicManager.PlayClip(sfx[9], true);
    }
}
