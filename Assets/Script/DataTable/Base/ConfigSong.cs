using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ConfigSongRecord
{
    //id	name	type	totalNote	tempo	fileID
    public int id;
    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
    }
    [SerializeField]
    private string type;
    public string Type
    {
        get
        {
            return type;
        }
    }
    [SerializeField]
    private int totalNote;
    public int TotalNote
    {
        get
        {
            return totalNote;
        }
    }
    [SerializeField]
    private int tempo;
    public int Tempo
    {
        get
        {
            return tempo;
        }
    }
    [SerializeField]
    private int fileID;
    public int FileID
    {
        get
        {
            return fileID;
        }
    }

    [SerializeField]
    private bool isUnlock;
    public bool IsUnlock
    {
        get
        {
            return isUnlock;
        }
    }
}


public class ConfigSong : BYDataTable<ConfigSongRecord>
{
    public override ConfigCompare<ConfigSongRecord> DefineConfigCompare()
    {
        configCompare = new ConfigCompare<ConfigSongRecord>("id");
        return configCompare;
    }

    public List<ConfigSongRecord> GetAllRecordSong()
    {
        return records.ToList();
    }

    public List<string> GetAllSongNames()
    {
        return records.Select(r => r.Name).ToList();
    }

}
