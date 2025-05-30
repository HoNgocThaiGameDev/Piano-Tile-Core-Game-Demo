﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class ISound 
{
    public string name;
    public AudioSource source;
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
}
