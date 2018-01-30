using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXBoy : MonoBehaviour {
    public AudioClip[] sfx;
	
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1) && sfx.Length > 0)
            MusicManager.PlayClip(sfx[0], true);
        if (Input.GetKeyDown(KeyCode.Alpha2) && sfx.Length > 1)
            MusicManager.PlayClip(sfx[1], true);
        if (Input.GetKeyDown(KeyCode.Alpha3) && sfx.Length > 2)
            MusicManager.PlayClip(sfx[2], true);
        if (Input.GetKeyDown(KeyCode.Alpha4) && sfx.Length > 3)
            MusicManager.PlayClip(sfx[3], true);
        if (Input.GetKeyDown(KeyCode.Alpha5) && sfx.Length > 4)
            MusicManager.PlayClip(sfx[4], true);
        if (Input.GetKeyDown(KeyCode.Alpha6) && sfx.Length > 5)
            MusicManager.PlayClip(sfx[5], true);
        if (Input.GetKeyDown(KeyCode.Alpha7) && sfx.Length > 6)
            MusicManager.PlayClip(sfx[6], true);
        if (Input.GetKeyDown(KeyCode.Alpha8) && sfx.Length > 7)
            MusicManager.PlayClip(sfx[7], true);
        if (Input.GetKeyDown(KeyCode.Alpha9) && sfx.Length > 8)
            MusicManager.PlayClip(sfx[8], true);
        if (Input.GetKeyDown(KeyCode.Alpha0) && sfx.Length > 9)
            MusicManager.PlayClip(sfx[9], true);
    }
}
