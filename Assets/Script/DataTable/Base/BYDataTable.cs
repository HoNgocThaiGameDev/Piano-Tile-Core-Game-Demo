using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class BYDataBase : ScriptableObject
{
    public virtual void CreateBinaryFile(TextAsset csv_file)
    {

    }
}
public class ConfigCompare<T> : IComparer<T> where T: class,new()
{
    private List<FieldInfo> keyinfos = new List<FieldInfo>();
    public ConfigCompare(params string[] keyInfonames)
    {
        for (int i = 0; i < keyInfonames.Length;i++)
        {
            FieldInfo keyinfo = typeof(T).GetField(keyInfonames[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            keyinfos.Add(keyinfo);
        }
    }
    public int Compare(T x, T y)
    {
        int result = 0;
        for (int i = 0; i < keyinfos.Count; i++)
        {
            object val_a = keyinfos[i].GetValue(x);
            object val_b = keyinfos[i].GetValue(y);
            result = ((IComparable)val_a).CompareTo(val_b);
            if (result != 0)
                break;
        }
        return result;
    }

    public T SetValueSearch(params object[] values)
    {
        T key = new T();
        for (int i = 0; i < values.Length; i++)
        {
            keyinfos[i].SetValue(key, values[i]);
        }
        return key;
    }
}
public abstract class BYDataTable<T> : BYDataBase where T : class, new()
{
    [SerializeField]
    protected List<T> records = new List<T>();
    protected ConfigCompare<T> configCompare;
    public abstract ConfigCompare<T> DefineConfigCompare();

    private void OnEnable()
    {
        DefineConfigCompare();
    }


    public override void CreateBinaryFile(TextAsset csv_file)
    {
        DefineConfigCompare();
        records.Clear();
        List<List<string>> grids = SplitCsvFile(csv_file);
        Type recordType = typeof(T);
        FieldInfo[] fieldInfos = recordType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 1; i < grids.Count; i++)
        {
            List<string> dataline = grids[i];
            string json_string = "{";
            for (int x = 0; x < dataline.Count; x++)
            {
                if (x > 0)
                    json_string += ",";
                if (fieldInfos[x].FieldType != typeof(string))
                {
                    string data_field = "0";
                    if (x < dataline.Count)
                    {
                        if (dataline[x] != string.Empty)
                        {
                            data_field = dataline[x];
                        }
                    }

                    json_string += "\"" + fieldInfos[x].Name + "\":" + data_field;
                }
                else
                {
                    string data_field = string.Empty;
                    if (x < dataline.Count)
                    {
                        if (dataline[x] != string.Empty)
                        {
                            data_field = dataline[x];
                        }
                    }

                    json_string += "\"" + fieldInfos[x].Name + "\":\"" + data_field + "\"";
                }
            }
            json_string += "}";
           // Debug.LogError(json_string);
            T r = JsonUtility.FromJson<T>(json_string);
            records.Add(r);
            
        }
        records.Sort(configCompare);
    }

    private List<List<string>> SplitCsvFile(TextAsset csv_file)
    {
        List<List<string>> grids = new List<List<string>>();
        string[] lines = csv_file.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string s = lines[i];
            if (s.CompareTo(string.Empty) != 0)
            {
                string[] linedata = s.Split('\t');
                List<string> ls_string = new List<string>();
                foreach (string e in linedata)
                {
                    string newchar = Regex.Replace(e, @"\t|\n|\r", "");
                    newchar = Regex.Replace(newchar, @"""", "" + "");
                    ls_string.Add(newchar);
                }
                grids.Add(ls_string);
            }
        }
        return grids;
    }
    public List<T> GetAllRecord()
    {
        return records;
    }
    public T GetRecordByKeySearch(params object[] keys)
    {
        T key = configCompare.SetValueSearch(keys);
        int index = records.BinarySearch(key, configCompare);
        if(index>=0&&index<records.Count)
        {
            return records[index];
        }
        else
        {
            return null;
        }
    }
}
