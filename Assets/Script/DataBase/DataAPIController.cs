using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataAPIController", menuName = "BY/DataAPIController", order = 1)]
public class DataAPIController : ScriptableObject
{
    public static DataAPIController instance;
    [SerializeField]
    private DataModel dataModel;
    public void InitData(Action callback)
    {
        instance = this;
        dataModel.LoadDataLocal((isNew) =>
        {
            Debug.LogError(" isnew : " + isNew);
            callback?.Invoke();

        });
    }

    public SongRecordData GetBestScore(int id)
    {
        SongRecordData wp = dataModel.ReadDataDictionary<SongRecordData>(DataPath.DIC_SONGRECORD, id.Tokey());
        return wp;
    }

    public void UpdateBestScoreById(int id, int newScore)
    {
        SongRecordData newData = GetBestScore(id);
        newData.bestScore = newScore;
        dataModel.UpdateDataDictionary<SongRecordData>(DataPath.DIC_SONGRECORD,id.Tokey(), newData,null);
        
    }
}
