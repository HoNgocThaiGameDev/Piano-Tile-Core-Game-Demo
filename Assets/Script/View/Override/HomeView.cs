using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeView : BaseView
{
    public GameObject rowPrefab;
    public Transform rowHolder;
    public Image imgSettingOnOff;
    public Transform transSongList, transSetting;
    public bool isSetup=false;
    public Sprite sprOn, sprOff;

    List<ISong> songList;
    List<Row> rows;
    public override void Setup(ViewParam data)
    {
        songList = AudioController.instance.GetSongs();
        if(!isSetup)
        {
            CreateSongListRow();
            isSetup = true;
        }    
        transSongList.gameObject.SetActive(true);
    }
    public override void OnShowView()
    {
       base.OnShowView();
    }
    public override void OnHideView()
    {
        base.OnHideView();
    }

    public void CreateSongListRow()
    {
        rows = new List<Row>();
        for (int i = 0; i < songList.Count; i++)
        {
            GameObject go = Instantiate(rowPrefab, rowHolder);
            Row row = go.GetComponent<Row>();
            row.txtSerial.text = (i + 1).ToString();
            row.name = songList[i].name;
            row.UpdateRow(songList[i]);
            rows.Add(row);
        }
    }
}
