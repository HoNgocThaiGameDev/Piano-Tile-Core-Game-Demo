using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public static class DataTrigger
{
    private static Dictionary<string, UnityEvent<object>> dic_event = new Dictionary<string, UnityEvent<object>>();
    public static void RegisterValueChange(string path, UnityAction<object> delegatgeDataChange)
    {
        if (!dic_event.ContainsKey(path))
        {
            dic_event.Add(path, new UnityEvent<object>());
        }
        dic_event[path].AddListener(delegatgeDataChange);
    }
    public static void UnRegisterValueChange(string path, UnityAction<object> delegatgeDataChange)
    {
        if (dic_event.ContainsKey(path))
        {
            dic_event[path].RemoveListener(delegatgeDataChange);
        }

    }
    public static void TriggerValueChange(this string path, object data)
    {
        if (dic_event.ContainsKey(path))
        {
            dic_event[path].Invoke(data);
        }
    }
    public static void TriggerAllValueChange(this string path, object data)
    {
        foreach (KeyValuePair<string, UnityEvent<object>> kp in dic_event)
        {
            kp.Value.Invoke(null);
        }
    }

}
[CreateAssetMenu(fileName = "DataModel", menuName = "BY/DataModel", order = 1)]
public class DataModel : ScriptableObject
{
    private UserData userData;
    public void LoadDataLocal(Action<bool> callback)
    {
        if (LoadData())
        {
            callback(false);
        }
        else
        {
            userData = new UserData();
            Dictionary<string, SongRecordData> dic = new Dictionary<string, SongRecordData>();
            foreach(ConfigSongRecord record in ConfigManager.instance.configSong.GetAllRecordSong())
            {
                
                SongRecordData data = new SongRecordData { id = record.id, Name=record.Name , bestScore = 0 };
                dic.Add(record.id.Tokey(), data);
            }
            userData.dic_songRecord = dic;

            SaveData();
            callback(true);
        }
    }

    #region Read 
    public T ReadData<T>(string path)
    {

        List<string> paths = path.ConvertPathToList();

        object outData;
        ReadDataByPath(paths, userData, out outData);
        return (T)outData;
    }
    private void ReadDataByPath(List<string> paths, object data_read, out object data_out)
    {


        string p = paths[0];
        Type t = data_read.GetType();
        FieldInfo field = t.GetField(p);
        if (paths.Count == 1)
        {
            data_out = field.GetValue(data_read);
        }
        else
        {
            paths.RemoveAt(0);
            ReadDataByPath(paths, field.GetValue(data_read), out data_out);
        }
    }
    #endregion
    #region Read Dictionary
    public T ReadDataDictionary<T>(string path_dic, string key)
    {

        List<string> paths = path_dic.ConvertPathToList();

        T outData;
        ReadDataDictionaryByPath<T>(paths, key, userData, out outData);
        return (T)outData;
    }
    private void ReadDataDictionaryByPath<T>(List<string> paths, string key, object data_read, out T data_out)
    {


        string p = paths[0];
        Type t = data_read.GetType();
        FieldInfo field = t.GetField(p);
        if (paths.Count == 1)
        {
            object dic = field.GetValue(data_read);
            Dictionary<string, T> dic_data = (Dictionary<string, T>)dic;
            dic_data.TryGetValue(key, out data_out);
        }
        else
        {
            paths.RemoveAt(0);
            ReadDataDictionaryByPath<T>(paths, key, field.GetValue(data_read), out data_out);
        }
    }
    #endregion
    #region Update 
    public void UpdateData(string path, object data_new, Action callback = null)
    {

        List<string> paths = path.ConvertPathToList();

        UpdateDataByPath(paths, userData, data_new, callback);
        path.TriggerValueChange(data_new);
        SaveData();
    }
    private void UpdateDataByPath(List<string> paths, object data_update, object data_new, Action callback)
    {


        string p = paths[0];
        Type t = data_update.GetType();
        FieldInfo field = t.GetField(p);
        if (paths.Count == 1)
        {
            field.SetValue(data_update, data_new);
            if (callback != null)
                callback();
        }
        else
        {
            paths.RemoveAt(0);
            UpdateDataByPath(paths, field.GetValue(data_update), data_new, callback);
        }
    }
    #endregion
    #region Update Dictionary
    public void UpdateDataDictionary<T>(string path_dic, string key, T data_new, Action callback = null)
    {

        List<string> paths = path_dic.ConvertPathToList();
        object dic_change;
        UpdateDataDictionaryByPath<T>(paths, key, userData, data_new, out dic_change, callback);
        path_dic.TriggerValueChange(dic_change);
        (path_dic + "/" + key).TriggerValueChange(data_new);
        SaveData();
    }
    private void UpdateDataDictionaryByPath<T>(List<string> paths, string key, object data_update, T data_new, out object dic_change, Action callback)
    {


        string p = paths[0];
        Type t = data_update.GetType();
        FieldInfo field = t.GetField(p);
        if (paths.Count == 1)
        {

            object dic = field.GetValue(data_update);
            Dictionary<string, T> dic_data = (Dictionary<string, T>)dic;
            if (dic_data == null)
            {
                dic_data = new Dictionary<string, T>();
            }

            dic_data[key] = data_new;
            dic_change = dic_data;
            field.SetValue(data_update, dic_data);
            if (callback != null)
                callback();
        }
        else
        {
            paths.RemoveAt(0);
            UpdateDataDictionaryByPath<T>(paths, key, field.GetValue(data_update), data_new, out dic_change, callback);
        }
    }
    #endregion
    #region Load
    private void SaveData()
    {
        string s_data = JsonConvert.SerializeObject(userData);
        PlayerPrefs.SetString("DATA", s_data);
    }
    private bool LoadData()
    {
        if (PlayerPrefs.HasKey("DATA"))
        {
            string s_data = PlayerPrefs.GetString("DATA");
            if (s_data.Length < 10)
            {
                return false;
            }
            else
            {
                userData = JsonConvert.DeserializeObject<UserData>(s_data);
                return true;
            }

        }
        return false;
    }
    #endregion
}
