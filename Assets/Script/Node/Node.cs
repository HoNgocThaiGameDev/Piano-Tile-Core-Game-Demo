using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{

    public enum TypeNodeSong { DEAD, START, NORMAL, BOMB, LONG, LONG2, LONG3, MIXED };

    public Image longImg2;
    public Sprite normalImg, clickedImg, startImg, longImg, deadImg;
    public Text txtStart, txtPopupScore;
    public TypeNodeSong type;

    public System.Action<int> OnClicked;
    public System.Action<Node, bool> OnLeave;

    public float height = 300f, width = 200f;
    float scoreLong = 0;

    [SerializeField]
    bool isClicked, isLongType, isThumps, isChecked;
    bool isBomb;
    RectTransform rectTrans, longRect, txtRect;

    public Transform longB, bomb;
    public Transform longTrans;
    //public string note = "a2";
    public int nodeIndex;
    int toneIdex;

    float normalHeight = 300f;
    float longHeight = 700f;
    float long2Height = 1100f;
    float long3Height = 1400f;


    void Update()
    {
        if (!InGameView.Instance.isMoveable()) return;
        Move();
        LongRectSize();
        CheckPos();
        Disable();

    }

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    public float GetScoreMultiplier()
    {
        switch (type)
        {
            case TypeNodeSong.LONG: return 8f;
            case TypeNodeSong.LONG2: return 12f;
            case TypeNodeSong.LONG3: return 16f;
            default: return 0f;
        }
    }


    float GetHeight(TypeNodeSong type)
    {
        float _height = 0;
        switch (type)
        {
            case Node.TypeNodeSong.START:
                _height = normalHeight;
                break;
            case Node.TypeNodeSong.NORMAL:
                _height = normalHeight;
                break;
            case Node.TypeNodeSong.BOMB:
                _height = normalHeight;
                break;
            case Node.TypeNodeSong.MIXED:
                _height = normalHeight;
                break;
            case Node.TypeNodeSong.LONG:
                _height = longHeight;
                break;
            case Node.TypeNodeSong.LONG2:
                _height = long2Height;
                break;
            case Node.TypeNodeSong.LONG3:
                _height = long3Height;
                break;


        }
        return _height;
    }

    public void Init(TypeNodeSong type, int nodeIndex,bool isDead)
    {
        this.type = type;
        this.nodeIndex = nodeIndex;
        this.toneIdex = nodeIndex;
        isBomb = false;

        height = GetHeight(type);
        // this.note = note.music;

        rectTrans = GetComponent<RectTransform>();
        txtRect = transform.GetChild(1).GetComponent<RectTransform>();
        switch (type)
        {
            
            case TypeNodeSong.START:
                longTrans.gameObject.SetActive(false);
                GetComponent<Image>().sprite = startImg;
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

                longB.gameObject.SetActive(false);
                txtStart.gameObject.SetActive(true);


                break;
            case TypeNodeSong.NORMAL:
                longTrans.gameObject.SetActive(false);
                GetComponent<Image>().sprite = normalImg;
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                txtStart.gameObject.SetActive(false);
                longB.gameObject.SetActive(false);
                bomb.gameObject.SetActive(false);
                break;
            case TypeNodeSong.BOMB:
                longTrans.gameObject.SetActive(false);
                GetComponent<Image>().sprite = normalImg;
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                txtStart.gameObject.SetActive(false);
                longB.gameObject.SetActive(false);
                bomb.gameObject.SetActive(true);
                isBomb = true;
                break;

            case TypeNodeSong.MIXED:
                longTrans.gameObject.SetActive(false);
                GetComponent<Image>().sprite = normalImg;
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                txtStart.gameObject.SetActive(false);
                longB.gameObject.SetActive(false);
                break;
            case TypeNodeSong.LONG:
                longTrans.gameObject.SetActive(true);
                longB.gameObject.SetActive(true);
                GetComponent<Image>().sprite = longImg;
                longRect = longTrans.GetComponent<RectTransform>();
                txtStart.gameObject.SetActive(false);
                isLongType = true;
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                longRect.sizeDelta = new Vector2(width, height / 4);
                break;
            case TypeNodeSong.LONG2:
                longTrans.gameObject.SetActive(true);
                longB.gameObject.SetActive(true);
                GetComponent<Image>().sprite = longImg;
                longRect = longTrans.GetComponent<RectTransform>();
                txtStart.gameObject.SetActive(false);
                isLongType = true;
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                longRect.sizeDelta = new Vector2(width, height / 4);

                longImg2.gameObject.SetActive(true);
                longImg2.GetComponent<RectTransform>().localPosition = new Vector2(width / 2f, height / 1.5f);
                break;
            case TypeNodeSong.LONG3:
                longTrans.gameObject.SetActive(true);
                longB.gameObject.SetActive(true);
                GetComponent<Image>().sprite = longImg;
                longRect = longTrans.GetComponent<RectTransform>();
                txtStart.gameObject.SetActive(false);
                isLongType = true;
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                longRect.sizeDelta = new Vector2(width, height / 4);

                longImg2.gameObject.SetActive(true);
                longImg2.GetComponent<RectTransform>().localPosition = new Vector2(width / 2, height / 2);
                break;
        }

        if (isDead)
        {

            longTrans.gameObject.SetActive(false);
            GetComponent<Image>().sprite = deadImg;
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

            longB.gameObject.SetActive(false);
            txtStart.gameObject.SetActive(false);
        }
    }

    void Move()
    {
        transform.Translate(new Vector3(0, -1, 0) * InGameView.Instance.getSpeedValue() * Time.deltaTime);
    }
    public void OnClickDown()
    {

        if (isClicked) return;


        switch (type)
        {
            case TypeNodeSong.START:
                ClickedStart();

                break;
            case TypeNodeSong.NORMAL:
                ClickedNormal();
                break;
            case TypeNodeSong.BOMB:
                ClickedNormal();
                break;
            case TypeNodeSong.LONG:
                ClickedLong();
                break;
            case TypeNodeSong.LONG2:
                ClickedLong();
                break;
            case TypeNodeSong.LONG3:
                ClickedLong();
                break;
            case TypeNodeSong.MIXED:
                ClickedNormal();
                break;
        }
        if (!InGameView.Instance.isStared) return;

        isClicked = true;

        if (OnClicked != null)
        {

            OnClicked(toneIdex);
        }

    }
    public void OnLongImage()
    {
        toneIdex++;
        if (OnClicked != null)
        {
            OnClicked(toneIdex);
        }
    }
    public void OnClickUp()
    {
        //if (!InGameView.Instance.isStared) return;

        //isThumps = false;
        //if (isLongType)
        //{
        //    txtPopupScore.gameObject.SetActive(true);
        //    txtPopupScore.text = "+" + (int)scoreLong;
        //}
        //InGameView.Instance.UpdateScore((int)scoreLong);

        //scoreLong = 0;
        if (!InGameView.Instance.isStared) return;

        isThumps = false;

        int finalScore = Mathf.RoundToInt(scoreLong+1);
        InGameView.Instance.UpdateScore(finalScore);

        txtPopupScore.gameObject.SetActive(true);
        txtPopupScore.text = "+" + finalScore;

        scoreLong = 0; // reset 
    }
    void ClickedStart()
    {
        isThumps = true;
        txtStart.gameObject.SetActive(false);
        GetComponent<Image>().sprite = clickedImg;
        InGameView.Instance.StartGame();
        AudioController.instance.PlayFullMusic();

    }

    void ClickedNormal()
    {
        if (!InGameView.Instance.isStared) return;
        if (isBomb)
        {
            InGameView.Instance.Dead(this);
        }
        isThumps = true;
        if (!isClicked)
        {
            GetComponent<Image>().sprite = clickedImg;
            InGameView.Instance.UpdateScore(1);
        }
    }

    void ClickedLong()
    {
        if (!InGameView.Instance.isStared) return;
        isThumps = true;
    }

    void LongRectSize()
    {
        if (isLongType && isThumps && longRect.sizeDelta.y < height)
        {
            //longRect.sizeDelta = new Vector2(longRect.sizeDelta.x, longRect.sizeDelta.y + InGameView.Instance.speed / 70f);
            //scoreLong += 6 * Time.deltaTime;

            //InGameView.Instance.UpdateScore((int)deltaScore); 
            longRect.sizeDelta = new Vector2(
            longRect.sizeDelta.x,
            longRect.sizeDelta.y + InGameView.Instance.speed / 70f);

            float deltaScore = GetScoreMultiplier() * Time.deltaTime;
            scoreLong += deltaScore;

        }


    }
    void CheckPos()
    {
        if (isBomb && !isClicked && !isChecked && transform.position.y < -150f)
        {
            if (OnLeave != null)
            {
                OnLeave(this, true);
                isChecked = true;

                return;
            }
        }
        else if (isClicked && !isChecked && transform.position.y < -150f)
        {
            if (OnLeave != null)
            {
                OnLeave(this, true);
                isChecked = true;
                return;
            }

        }
        else if (OnLeave != null && !isClicked && !isChecked && transform.position.y < -150f)
        {
            OnLeave(this, false);
            isChecked = true;
        }
    }

    public void ResetState()
    {
        isClicked = false;
        isLongType = false;
        isThumps = false;
        isChecked = false;
        isBomb = false;
        scoreLong = 0;
        OnClicked = null;
        OnLeave = null;

        if (txtPopupScore != null)
            txtPopupScore.gameObject.SetActive(false);

        if (txtStart != null)
            txtStart.gameObject.SetActive(false);

        if (longImg2 != null)
            longImg2.gameObject.SetActive(false);

        if (longTrans != null)
            longTrans.gameObject.SetActive(false);

        if (longB != null)
            longB.gameObject.SetActive(false);

        if (bomb != null)
            bomb.gameObject.SetActive(false);

        // Reset sprite và size
        var img = GetComponent<Image>();
        if (img != null)
        {
            img.sprite = normalImg;
        }

        GetComponent<RectTransform>().sizeDelta = new Vector2(width, normalHeight);
        if (longRect != null)
        {
            longRect.sizeDelta = new Vector2(width, 0);
        }
    }
    void Disable()
    {
        if (!isClicked) return;
        if (transform.position.y < -2 * height)
        {
            InGameView.Instance.activeNodeList.Remove(this);
            BYPoolManager.instance.GetPool("Node").DeSpawned(this.transform);
        }
    }

}