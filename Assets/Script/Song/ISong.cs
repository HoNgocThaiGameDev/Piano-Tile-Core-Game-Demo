using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ISong
{
    public int id;
    public string name, type;
    public int totalNote = 30;
    public int tempo;
    public bool isUnlock;
    public AudioClip file;
}
