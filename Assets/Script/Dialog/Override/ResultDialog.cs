using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDialog : BaseDialog
{
    public Text txtIngameScore;
    public Text txtBestScore;

    public override void OnShowDialog()
    {
        DataTrigger.RegisterValueChange(DataPath.DIC_SONGRECORD, OnUpgradeNewScore);

        Time.timeScale = 0;
        BYPoolManager.instance.GetPool("Node").DeSpawnAll();
        OnUpgradeNewScore(null);
    }
    public override void OnHideDialog()
    {
        DataTrigger.UnRegisterValueChange(DataPath.DIC_SONGRECORD, OnUpgradeNewScore);
        Time.timeScale = 1;
    }

    public void OnQuit()
    {
        DialogManager.instance.HideDialog(dialogIndex);
        LoadSceneManager.instance.LoadSceneByIndex(1, () =>
        {
            AudioController.instance.Stop();
            InGameView.Instance.OnQuit();
            ViewManager.instance.SwitchView(ViewIndex.HomeView);
        });
    }

    public void OnRestart()
    {
        AudioController.instance.Stop();
        InGameView.Instance.OnRestartGame();
        DialogManager.instance.HideAllDialog();
    }

    public void OnUpgradeNewScore(object dataChange)
    {
        txtIngameScore.text = InGameView.Instance.txtInGameScore.text;
        SongRecordData songRecordData = DataAPIController.instance.GetBestScore(GameController.instance.SONG_ID);
        txtBestScore.text = songRecordData.bestScore.ToString();
        if (InGameView.Instance.score > songRecordData.bestScore)
        {
            DataAPIController.instance.UpdateBestScoreById(GameController.instance.SONG_ID, InGameView.Instance.score);
        }
    }    
}
