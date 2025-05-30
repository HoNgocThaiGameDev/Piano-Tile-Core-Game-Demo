using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SongRecordData
{
    public int id;
    public string Name;
    public int bestScore;
}

[Serializable]
public class UserData
{
    [SerializeField]
    public Dictionary<string, SongRecordData> dic_songRecord = new Dictionary<string, SongRecordData>();
 
}

public class DataPath
{
    public const string DIC_SONGRECORD = "dic_songRecord";
}