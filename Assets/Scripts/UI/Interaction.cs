using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 交互面板
/// </summary>
public class Interaction : MonoBehaviour
{
    private GameObject deal;
    private GameObject play;
    private GameObject disard;
    private GameObject grab;
    private GameObject disgrab;
    private GameController controller;
    // Use this for initialization
    void Start()
    {
        deal = gameObject.transform.Find("DealBtn").gameObject;
        play = gameObject.transform.Find("PlayBtn").gameObject;
        disard = gameObject.transform.Find("DiscardBtn").gameObject;
        grab = gameObject.transform.Find("GrabBtn").gameObject;
        disgrab = gameObject.transform.Find("DisgrabBtn").gameObject;
        controller = GameObject.Find("GameController").GetComponent<GameController>();

        deal.GetComponent<UIButton>().onClick.Add(new EventDelegate(DealCallBack));
        play.GetComponent<UIButton>().onClick.Add(new EventDelegate(PlayCallBack));
        disard.GetComponent<UIButton>().onClick.Add(new EventDelegate(DiscardCallBack));
        grab.GetComponent<UIButton>().onClick.Add(new EventDelegate(GrabLordCallBack));
        disgrab.GetComponent<UIButton>().onClick.Add(new EventDelegate(DisgrabLordCallBack));

        //激活出牌按钮事件绑定
        OrderController.Instance.activeButton += ActiveCardButton;

        play.SetActive(false);
        disard.SetActive(false);
        grab.SetActive(false);
        disgrab.SetActive(false);
    }

    /// <summary>
    /// 激活出牌按钮
    /// </summary>
    /// <param name="canReject"></param>
    void ActiveCardButton(bool canReject)
    {
        play.SetActive(true);
        disard.SetActive(true);

        disard.GetComponent<UIButton>().isEnabled = canReject;
    }

    /// <summary>
    /// 发牌回调
    /// </summary>
    public void DealCallBack()
    {
        controller.DealCards();
        //抢地主出现
        grab.SetActive(true);
        disgrab.SetActive(true);
        deal.SetActive(false);
    }

    /// <summary>
    /// 出牌回调
    /// </summary>
    void PlayCallBack()
    {
        PlayCard playCard = GameObject.Find("Player").GetComponent<PlayCard>();
        if (playCard.CheckSelectCards())
        {
            play.SetActive(false);
            disard.SetActive(false);
        }
    }

    /// <summary>
    /// 不出
    /// </summary>
    void DiscardCallBack()
    {
        OrderController.Instance.Turn();
        play.SetActive(false);
        disard.SetActive(false);
    }

    /// <summary>
    /// 抢地主
    /// </summary>
    void GrabLordCallBack()
    {
        //玩家的地主
        controller.CardsOnTable(CharacterType.Player);
        OrderController.Instance.Init(CharacterType.Player);
        grab.SetActive(false);
        disgrab.SetActive(false);
    }

    /// <summary>
    /// 不抢
    /// </summary>
    void DisgrabLordCallBack()
    {
        int index = Random.Range(2, 4);
        controller.CardsOnTable((CharacterType)index);
        OrderController.Instance.Init((CharacterType)index);
        grab.SetActive(false);
        disgrab.SetActive(false);
    }

}
