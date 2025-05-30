using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDialog : BaseDialog
{
    // Start is called before the first frame update
    public override void OnShowDialog()
    {
        Time.timeScale = 0;
        InGameView.Instance.bestScoreObj.SetActive(true);
    }
    public override void OnHideDialog()
    {
        Time.timeScale = 1;
        InGameView.Instance.bestScoreObj.SetActive(false);
    }
    public void OnResume()
    {
        AudioController.instance.ResumeMusic();
        DialogManager.instance.HideDialog(dialogIndex);
        InGameView.Instance.bestScoreObj.SetActive(false);
    }
    public void OnQuit()
    {
        AudioController.instance.Stop();
        DialogManager.instance.HideDialog(dialogIndex);
        LoadSceneManager.instance.LoadSceneByIndex(1, () =>
        {
            InGameView.Instance.OnQuit();
            ViewManager.instance.SwitchView(ViewIndex.HomeView);
            InGameView.Instance.bestScoreObj.SetActive(false);
        });
    }

    public void OnRestart()
    {
        AudioController.instance.Stop();
        InGameView.Instance.OnRestartGame();
        DialogManager.instance.HideDialog(DialogIndex.PauseDialog);
        InGameView.Instance.bestScoreObj.SetActive(true);
    }

}
