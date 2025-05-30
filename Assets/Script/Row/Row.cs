using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour
{
    public Text txtSerial, txtTitle, txtType;

    public Image bg;
    public Sprite sprFavActive, sprFavInactive,sprLock,sprNormal;
    public bool isUnlock;
    public string songName;
    public int id;
    public Image raycastUnlock;

    public void UpdateRow(ISong song)
    {
        txtTitle.text = song.name;
        txtType.text = song.type;
        songName = song.name;
        isUnlock = song.isUnlock;
        id = song.id;
        bg.sprite = !isUnlock ? sprLock : sprNormal;
        raycastUnlock.raycastTarget = isUnlock ? true : false;
    }
    public void OnPlayClick()
    {
        GameController.instance.SONG_NAME = songName;
        Debug.LogError(GameController.instance.SONG_NAME);
        GameController.instance.SONG_ID = id;
        GameController.instance.TYPE_MUSIC = txtType.text;
        Debug.LogError(GameController.instance.SONG_ID);
        ViewManager.instance.SwitchView(ViewIndex.InGameView);
    }
}