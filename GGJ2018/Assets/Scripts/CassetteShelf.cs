using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteShelf : MonoBehaviour {
    public static CassetteShelf Instance = null;
    public Transform[] cassettes;
    public Texture[] textures;
    public AudioClip tick;
	// Use this for initialization
	void Start () {
        Instance = this;
        cassettes = new Transform[transform.GetChild(1).childCount];
        for(int i =0; i < transform.GetChild(1).childCount; i++)
        {
            cassettes[i] = transform.GetChild(1).GetChild(i);
            cassettes[i].GetChild(0).GetComponent<Renderer>().material.mainTexture = textures[i];
        }

        Vector3 pos;
        for (int i = 0; i < cassettes.Length; i++)
        {
            pos = cassettes[i].localPosition;
            pos.z = -2f;
            cassettes[i].localPosition = pos;
        }
    }
	
    public void SetTape(int index)
    {
        MusicManager.PlayClip(tick);
        Debug.Log("Set tape: " + index);
        Vector3 pos;
        for (int i=0; i < cassettes.Length; i++)
        {
            pos = cassettes[i].localPosition;
            pos.z = -2f;
            cassettes[i].localPosition = pos;
        }

         pos = cassettes[index].localPosition;
        pos.z = -8f;
        cassettes[index].localPosition = pos;
    }
    public Texture GetTex(int index)
    {
        return textures[index];
    }
	// Update is called once per frame
	void Update () {
		
	}
}
