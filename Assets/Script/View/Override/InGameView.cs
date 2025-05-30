using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Node;

public class InGameView : BaseView
{
    public static InGameView Instance;
    [Header("Prefab")]
    public GameObject nodePrefab;
    public GameObject bestScoreObj; 

    [Header("Transform")]
    public Transform anchorNode;
    public Transform transBestScore;
    Transform nodeHolder;

    [Header("Variables")]
    float scrHeight, scrWidth, devidedValue;
    private int songIndex;
    public bool isDead = false;
    public bool isStared, isTap = true;
    bool isPaused;
    float nodeWidth = 200f;
    float lastMixedNodePosY = 0;
    public int score =0;
    int randIindex, nodeIndex;
    public float speed = 0f;
    public const string NameNodePool = "Node";

    [Header("Score")]
    public Text txtInGameScore;
    public Text txtBestScore;
    public Text txtSongName;
    public Text txtTypeMusic;

    [Header("Node")]
    public List<Node> activeNodeList = new List<Node>();
    private int mixedNodeCount, spwanCompleted;
    Node lastNode, deadNode;

    [Header("Song")]
    public ISong activeSong;


    [Header("BackGround")]
    public Image background;
    private IEnumerator ienChangeBg;
    public List<Sprite> backgroundImgList;
    private int intervalCount;
    public float intervalTime = 10f;


    private void Start()
    {
        Instance = this;
        scrWidth = Screen.width;
        scrHeight = Screen.height;
        devidedValue = scrWidth / 4;
    }
    public override void Setup(ViewParam data)
    {
        txtSongName.text = GameController.instance.SONG_NAME;
        txtTypeMusic.text =GameController.instance.TYPE_MUSIC;
        base.Setup(data);
        SetupSong(GameController.instance.SONG_NAME);
        bestScoreObj.SetActive(true);
    }
    public override void OnShowView()
    {
        base.OnShowView();
        UpdateScore(0);
        txtBestScore.text = " Highest Score: " + DataAPIController.instance.GetBestScore(GameController.instance.SONG_ID).bestScore.ToString();
    }
    public override void OnHideView()
    {
        base .OnHideView();
        //animation
    }

    public void OnPause()
    {
        AudioController.instance.PauseFullMusic();
        DialogManager.instance.ShowDialog(DialogIndex.PauseDialog);
    }    

    public void OnRestartGame()
    {
        ResetGame();
        ViewManager.instance.SwitchView(ViewIndex.InGameView);
    }    

    public void OnQuit()
    {
        AudioController.instance.PauseFullMusic();
        ResetGame();
        ViewManager.instance.SwitchView(ViewIndex.HomeView);
    }  
    
    public void SetupSong(string songName)
    {
        songIndex = GameController.instance.SONG_ID - 1;
        GenerateGame();
    }


    #region GameCore

    private void GenerateGame()
    {
        if (nodeHolder == null)
        {
            nodeHolder = new GameObject("NodeHolder").transform;
            nodeHolder.parent = anchorNode;
            nodeHolder.localScale = Vector3.one;
        }

        for (int i = -1; i < 8; i++)
        {
            GenerateSingleNote(i);
        }

    }
    public void ResetGame()
    {
        scrHeight = Screen.height;
        nodeWidth = 200;


        score = 0;
        nodeIndex = 0;
        lastMixedNodePosY = 0;
        isStared = false;
        isDead = false;
        isPaused = false;
        isTap = true;


        if (nodeHolder != null)
        {
            Debug.LogError("enter holder");
            Destroy(nodeHolder.gameObject);
        }
        nodeHolder = null;
        activeNodeList.Clear();
        Color c = background.color;
        c.a = 1;
        background.color = c;
        UpdateBestRecord();
    }    

    TypeNodeSong GetRandomTypeNote()
    {
        Node.TypeNodeSong type = Node.TypeNodeSong.NORMAL;

        int v = Random.Range(0, 4);
        if (v == 0)
        {
            int t = Random.Range(0, 3);
            if (t == 0 )
                type = Node.TypeNodeSong.BOMB;
            else
                type = Node.TypeNodeSong.NORMAL;
        }
        else if (v == 1)
        {
            type = Node.TypeNodeSong.LONG;
        }
        else if (v == 2)
        {
            type = Node.TypeNodeSong.LONG2;
        }
        else if (v == 3)
        {
            type = Node.TypeNodeSong.LONG3;
        }
        return type;
    }

    void PlayMusicGame(int toneIndex)
    {
        if (!isStared) return;
        InGameView.Instance.bestScoreObj.SetActive(false);
        isPaused = false;
        AudioController.instance.ResumeMusic();
    }

    IEnumerator IGenerateDeadNote(Node dn, bool isMiss)
    {
        isTap = false;
        if (isMiss)
        {
            speed = 0;
        }
        else
        {
            speed = -speed;
            yield return new WaitForSeconds(0.5f);
            speed = 0;
            dn.gameObject.SetActive(false);

            dn = GenerateDeadNote(dn.transform.localPosition);
        }

        isDead = true;

        for (int i = 0; i < 4; i++)
        {

            dn.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);

            dn.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);


        }
        yield return new WaitForSeconds(1f);

        isStared = false;
        DialogManager.instance.ShowDialog(DialogIndex.ResultDialog);
        bestScoreObj.SetActive(true);
    }

    public void OnTapLeave(Node node, bool isComplete)
    {

        if (!isComplete)
        {
            deadNode = node;

            StartCoroutine(IGenerateDeadNote(node, false));

        }
        else if (isComplete)
        {

            if (nodeIndex < activeSong.totalNote)
                GenerateSingleNote(nodeIndex);
            else
            {

                nodeIndex = 0;
                spwanCompleted++;
            }
        }
    }

    void GenerateSingleNote(int index)
    {

        Node.TypeNodeSong activeType = index == -1 ? Node.TypeNodeSong.START : GetRandomTypeNote();
        float xPos = nodeWidth * GetRandomIdx();
        float yPos = 0;

        if (activeType == Node.TypeNodeSong.START)
        {
            yPos = transBestScore.GetComponent<RectTransform>().sizeDelta.y;
        }
        else if (activeType == Node.TypeNodeSong.MIXED)
        {
            mixedNodeCount++;
            if (mixedNodeCount >= 2)
            {
                yPos = lastMixedNodePosY;
                mixedNodeCount = 0;
            }
            else
            {
                yPos = lastNode.transform.localPosition.y + lastNode.height;
                lastMixedNodePosY = yPos;
            }
        }
        else
        {
            yPos = lastNode.transform.localPosition.y + lastNode.height;

        }
        Vector3 pos = new Vector3(xPos, yPos, 0);

        Transform trans = BYPoolManager.instance.GetPool(NameNodePool).Spawned();
        GameObject go = trans.gameObject;
        Debug.LogError("enter");
        trans.SetParent(nodeHolder);
        trans.GetComponent<RectTransform>().localPosition = pos;
        trans.GetComponent<RectTransform>().localScale = Vector3.one;

        lastNode = go.GetComponent<Node>();
        lastNode.Init(activeType, index, false);
        if (index > -1)
        {
            //Debug.LogError("enter");
            lastNode.OnClicked += PlayMusicGame;
        }
        lastNode.OnLeave += OnTapLeave;
        activeNodeList.Add(lastNode);

        nodeIndex++;
    }

    int GetRandomIdx()
    {
        int tmpIndex = Random.Range(0, 4);
        while (randIindex == tmpIndex)
        {
            tmpIndex = Random.Range(0, 4);
        }
        randIindex = tmpIndex;
        return randIindex;
    }

    Node GenerateDeadNote(Vector3 position)
    {
        AudioController.instance.Stop();

        Transform trans = BYPoolManager.instance.GetPool(NameNodePool).Spawned();
        GameObject go = trans.gameObject;
        //Debug.LogError("enter");
        trans.SetParent(nodeHolder);
        trans.GetComponent<RectTransform>().localPosition = position;
        trans.GetComponent<RectTransform>().localScale = Vector3.one;

        Node n = go.GetComponent<Node>();
        n.Init(deadNode.type, deadNode.nodeIndex, true);

        return n;
    }

    public void OnTap()
    {
        if (!isStared || isDead || !isTap || isPaused) return;

        float clickPosX = Input.mousePosition.x;
        float clickPosY = Input.mousePosition.y;

        for (int i = 0; i < activeNodeList.Count; i++)
        {
            if (activeNodeList[i].transform.localPosition.y < clickPosY && clickPosY < (activeNodeList[i].height + activeNodeList[i].transform.localPosition.y))
            {

                deadNode = activeNodeList[i];
                int index = (int)(clickPosX / devidedValue);
                Vector3 deadPos = new Vector3(index * nodeWidth, activeNodeList[i].transform.localPosition.y, 0);
                speed = 0;

                Node dn = GenerateDeadNote(deadPos);
                StartCoroutine(IGenerateDeadNote(dn, true));
                return;
            }
        }
    }

    public void Dead(Node deadNode)
    {
        this.deadNode = deadNode;
        StartCoroutine(IGenerateDeadNote(deadNode, false));
    }


    #endregion

    #region Extensions
    public float getSpeedValue()
    {
        return speed;
    }

    public bool isMoveable()
    {
        return !isDead && !isPaused && isStared;
    }
    
    #endregion

    #region System Game
    public void StartGame()
    {
        isStared = true;
        activeSong = AudioController.instance.GetSong(GameController.instance.SONG_NAME);
        speed = activeSong.tempo * scrHeight / 1280f;
        Invoke("ChangeParallaxBackground", intervalTime);

    }

    void ChangeParallaxBackground()
    {
        if (isPaused || isDead || !isStared) return;

        if (ienChangeBg != null)
            StopCoroutine(ienChangeBg);
        ienChangeBg = IChangeBG(backgroundImgList[intervalCount]);
        StartCoroutine(ienChangeBg);
        intervalCount++;
        if (intervalCount >= backgroundImgList.Count)
        {
            intervalCount = 0;
        }

    }

    IEnumerator IChangeBG(Sprite sprite)
    {

        Color c = background.color;

        while (c.a > 0.1f)
        {
            c.a -= 0.01f;
            yield return null;
            background.color = c;
        }
        background.sprite = sprite;
        yield return new WaitForSeconds(1);
        while (c.a < 1f)
        {
            c.a += 0.01f;
            yield return null;
            background.color = c;
        }
        CancelInvoke("ChangeParallaxBackground");
        Invoke("ChangeParallaxBackground", intervalTime);
    }

    #endregion

    #region Score
    public void UpdateScore(int amount)
    {
        score += amount;
        txtInGameScore.text = score.ToString();
    }

    public void UpdateBestRecord()
    {
        ISong song = AudioController.instance.GetSong(GameController.instance.SONG_NAME);
        transBestScore.GetComponent<RectTransform>().position = Vector3.zero;
        txtSongName.text = song.name;
        txtTypeMusic.text = song.type;
        txtBestScore.text =" Highest Score: " + DataAPIController.instance.GetBestScore(GameController.instance.SONG_ID).ToString();
    }
    #endregion
}
