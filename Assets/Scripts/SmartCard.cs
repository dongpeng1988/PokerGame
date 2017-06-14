using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 电脑出牌AI
/// </summary>
public abstract class SmartCard : MonoBehaviour
{
    protected GameObject computerNotice;
    // Use this for initialization
    void Start()
    {
        computerNotice = transform.Find("ComputerNotice").gameObject;
        OrderController.Instance.smartCard += AutoDiscardCard;
    }

    /// <summary>
    /// 自动出牌
    /// </summary>
    protected void AutoDiscardCard(bool isNone)
    {
        HandCards t = gameObject.GetComponent<HandCards>();
        if (OrderController.Instance.Type == t.cType)
        {
            StartCoroutine(DelayDiscardCard(isNone));

        }

    }

    /// <summary>
    /// 一手牌
    /// </summary>
    /// <returns></returns>
    public abstract List<Card> FirstCard();

    /// <summary>
    /// 延时出牌
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator DelayDiscardCard(bool isNone)
    {
        yield return new WaitForSeconds(1.0f);
        CardsType rule = isNone ? CardsType.None : DeskCardsCache.Instance.Rule;
        int deskWeight = DeskCardsCache.Instance.TotalWeight;
        //根据桌面牌的类型和权值大小出牌
        switch (rule)
        {
            case CardsType.None:
                List<Card> discardCards_00 = FirstCard();
                if (discardCards_00.Count != 0)
                {
                    RemoveCards(discardCards_00);
                    DiscardCards(discardCards_00, GetSprite(discardCards_00));
                }
                break;
            case CardsType.JokerBoom:
                OrderController.Instance.Turn();
                ShowNotice();
                break;
            case CardsType.Boom:
                List<Card> discardCards_01 = FindBoom(GetAllCards(), deskWeight, false);
                if (discardCards_01.Count != 0)
                {
                    GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
                    //积分翻倍
                    if (discardCards_01.Count == 4)
                    {
                        //普通炸弹2倍
                        controller.Multiples = 2;
                    }
                    else if (discardCards_01.Count == 2)
                    {
                        //王炸4倍
                        controller.Multiples = 4;
                    }
                    RemoveCards(discardCards_01);
                    DiscardCards(discardCards_01, GetSprite(discardCards_01));
                }
                else
                {
                    OrderController.Instance.Turn();
                    ShowNotice();
                }
                break;
            case CardsType.Double:
                List<Card> discardCards_02 = FindDouble(GetAllCards(), deskWeight, false);
                if (discardCards_02.Count != 0)
                {
                    RemoveCards(discardCards_02);
                    DiscardCards(discardCards_02, GetSprite(discardCards_02));
                }
                else
                {
                    OrderController.Instance.Turn();
                    ShowNotice();
                }
                break;
            case CardsType.Single:
                List<Card> discardCards_03 = FindSingle(GetAllCards(), deskWeight, false);
                if (discardCards_03.Count != 0)
                {
                    RemoveCards(discardCards_03);
                    DiscardCards(discardCards_03, GetSprite(discardCards_03));
                }
                else
                {
                    OrderController.Instance.Turn();
                    ShowNotice();
                }
                break;
            case CardsType.OnlyThree:
                List<Card> discardCards_04 = FindOnlyThree(GetAllCards(), deskWeight, false);
                if (discardCards_04.Count != 0)
                {
                    RemoveCards(discardCards_04);
                    DiscardCards(discardCards_04, GetSprite(discardCards_04));
                }
                else
                {
                    OrderController.Instance.Turn();
                    ShowNotice();
                }
                break;
            case CardsType.Straight:
                List<Card> discardCards_05 = FindStraight(GetAllCards(), DeskCardsCache.Instance.MinWeight, DeskCardsCache.Instance.CardsCount, false);
                if (discardCards_05.Count != 0)
                {
                    RemoveCards(discardCards_05);
                    DiscardCards(discardCards_05, GetSprite(discardCards_05));
                }
                else
                {
                    List<Card> boom = FindBoom(GetAllCards(), 0, true);
                    if (boom.Count != 0)
                    {
                        RemoveCards(boom);
                        DiscardCards(boom, GetSprite(boom));
                    }
                    else
                    {
                        OrderController.Instance.Turn();
                        ShowNotice();
                    }
                }
                break;
            case CardsType.ThreeAndOne:
                List<Card> discardCards_06 = FindThreeAndOne(GetAllCards(), deskWeight, false);
                if (discardCards_06.Count != 0)
                {
                    RemoveCards(discardCards_06);
                    DiscardCards(discardCards_06, GetSprite(discardCards_06));
                }
                else
                {
                    OrderController.Instance.Turn();
                    ShowNotice();
                }
                break;
            case CardsType.ThreeAndTwo:
                List<Card> discardCards_08 = FindThreeAndTwo(GetAllCards(), deskWeight, false);
                if (discardCards_08.Count != 0)
                {
                    RemoveCards(discardCards_08);
                    DiscardCards(discardCards_08, GetSprite(discardCards_08));
                }
                else
                {
                    OrderController.Instance.Turn();
                    ShowNotice();
                }
                break;
            case CardsType.DoubleStraight:
                List<Card> discardCards_10 = FindStraight(GetAllCards(), DeskCardsCache.Instance.MinWeight, DeskCardsCache.Instance.CardsCount, false);
                if (discardCards_10.Count != 0)
                {
                    RemoveCards(discardCards_10);
                    DiscardCards(discardCards_10, GetSprite(discardCards_10));
                }
                else
                {
                    List<Card> boom = FindBoom(GetAllCards(), 0, true);
                    if (boom.Count != 0)
                    {
                        RemoveCards(boom);
                        DiscardCards(boom, GetSprite(boom));
                    }
                    else
                    {
                        OrderController.Instance.Turn();
                        ShowNotice();
                    }
                }
                break;
            case CardsType.TripleStraight:
                List<Card> boom_01 = FindBoom(GetAllCards(), 0, true);
                if (boom_01.Count != 0)
                {
                    RemoveCards(boom_01);
                    DiscardCards(boom_01, GetSprite(boom_01));
                }
                else
                {
                    OrderController.Instance.Turn();
                    ShowNotice();
                }
                break;
        }
    }

    /// <summary>
    /// 出牌动画
    /// </summary>
    /// <param name="selectedCardsList"></param>
    /// <param name="selectedSpriteList"></param>
    protected void DiscardCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList)
    {
        Card[] selectedCardsArray = selectedCardsList.ToArray();
        //检测是否符合出牌规则
        CardsType type;
        if (CardRules.PopEnable(selectedCardsArray, out type))
        {

            HandCards player = gameObject.GetComponent<HandCards>();
            //如果符合将牌从手牌移到出牌缓存区
            DeskCardsCache.Instance.Clear();
            DeskCardsCache.Instance.Rule = type;

            for (int i = 0; i < selectedSpriteList.Count; i++)
            {
                DeskCardsCache.Instance.AddCard(selectedSpriteList[i].Poker);
                selectedSpriteList[i].transform.parent = GameObject.Find("Desk").transform;
                selectedSpriteList[i].Poker = selectedSpriteList[i].Poker;
            }

            DeskCardsCache.Instance.Sort();
            GameController.AdjustCardSpritsPosition(CharacterType.Desk);
            GameController.AdjustCardSpritsPosition(player.cType);

            GameController.UpdateLeftCardsCount(player.cType, player.CardsCount);

            if (player.CardsCount == 0)
            {
                GameObject.Find("GameController").GetComponent<GameController>().GameOver();
            }
            else
            {
                OrderController.Instance.Biggest = player.cType;
                OrderController.Instance.Turn();
            }
        }
    }

    /// <summary>
    /// 获取所有手牌
    /// </summary>
    /// <returns></returns>
    protected List<Card> GetAllCards(List<Card> exclude = null)
    {
        List<Card> cards = new List<Card>();
        HandCards allCards = gameObject.GetComponent<HandCards>();
        bool isContinue = false;
        for (int i = 0; i < allCards.CardsCount; i++)
        {
            isContinue = false;
            if (exclude != null)
            {
                for (int j = 0; j < exclude.Count; j++)
                {
                    if (allCards[i] == exclude[j])
                    {
                        isContinue = true;
                        break;
                    }

                }
            }

            if (!isContinue)
                cards.Add(allCards[i]);
        }
        //从小到大排序
        CardRules.SortCards(cards, true);
        return cards;
    }

    /// <summary>
    /// 获得card对应的精灵
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    protected List<CardSprite> GetSprite(List<Card> cards)
    {
        HandCards t = gameObject.GetComponent<HandCards>();
        CardSprite[] sprites = GameObject.Find(t.cType.ToString()).GetComponentsInChildren<CardSprite>();

        List<CardSprite> selectedSpriteList = new List<CardSprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            for (int j = 0; j < cards.Count; j++)
            {
                if (cards[j] == sprites[i].Poker)
                {
                    selectedSpriteList.Add(sprites[i]);
                    break;
                }
            }
        }

        return selectedSpriteList;
    }

    /// <summary>
    /// 移除手牌
    /// </summary>
    /// <param name="cards"></param>
    protected void RemoveCards(List<Card> cards)
    {
        HandCards allCards = gameObject.GetComponent<HandCards>();

        for (int j = 0; j < cards.Count; j++)
        {
            for (int i = 0; i < allCards.CardsCount; i++)
            {
                if (cards[j] == allCards[i])
                {
                    allCards.PopCard(cards[j]);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 找到手牌中符合要求的炸弹
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    protected List<Card> FindBoom(List<Card> allCards, int weight, bool equal)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i <= allCards.Count - 4)
            {
                //先找普通炸弹
                if (allCards[i].GetCardWeight == allCards[i + 1].GetCardWeight &&
                    allCards[i].GetCardWeight == allCards[i + 2].GetCardWeight &&
                    allCards[i].GetCardWeight == allCards[i + 3].GetCardWeight)
                {
                    int totalWeight = (int)allCards[i].GetCardWeight + (int)allCards[i + 1].GetCardWeight + (int)allCards[i + 2].GetCardWeight
                        + (int)allCards[i + 4].GetCardWeight;
                    if (equal)
                    {
                        if (totalWeight >= weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            ret.Add(allCards[i + 2]);
                            ret.Add(allCards[i + 3]);
                            break;
                        }
                    }
                    else
                    {
                        if (totalWeight > weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            ret.Add(allCards[i + 2]);
                            ret.Add(allCards[i + 3]);
                            break;
                        }
                    }

                }
            }
        }

        //找王炸
        if (ret.Count == 0)
        {
            for (int j = 0; j < allCards.Count; j++)
            {
                if (j < allCards.Count - 1)
                {
                    if (allCards[j].GetCardWeight == Weight.SJoker &&
                        allCards[j + 1].GetCardWeight == Weight.LJoker)
                    {
                        ret.Add(allCards[j]);
                        ret.Add(allCards[j + 1]);
                    }
                }

            }
        }

        return ret;
    }

    /// <summary>
    /// 找到手牌中符合要求的是对子
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    protected List<Card> FindDouble(List<Card> allCards, int weight, bool equal)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 1)
            {
                if (allCards[i].GetCardWeight == allCards[i + 1].GetCardWeight)
                {
                    int totalWeight = (int)allCards[i].GetCardWeight + (int)allCards[i + 1].GetCardWeight;
                    if (equal)
                    {
                        if (totalWeight >= weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            break;
                        }
                    }
                    else
                    {
                        if (totalWeight > weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            break;
                        }
                    }

                }
            }
        }

        return ret;
    }

    /// <summary>
    /// 找到手牌中符合要求的是单牌
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    protected List<Card> FindSingle(List<Card> allCards, int weight, bool equal)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (equal)
            {
                if ((int)allCards[i].GetCardWeight >= weight)
                {
                    ret.Add(allCards[i]);
                    break;
                }
            }
            else
            {
                if ((int)allCards[i].GetCardWeight > weight)
                {
                    ret.Add(allCards[i]);
                    break;
                }
            }

        }
        return ret;
    }

    /// <summary>
    /// 找到手牌中符合要求的是3章
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    protected List<Card> FindOnlyThree(List<Card> allCards, int weight, bool equal)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i <= allCards.Count - 3)
            {
                if (allCards[i].GetCardWeight == allCards[i + 1].GetCardWeight &&
                    allCards[i].GetCardWeight == allCards[i + 2].GetCardWeight)
                {
                    int totalWeight = (int)allCards[i].GetCardWeight +
                        (int)allCards[i + 1].GetCardWeight +
                        (int)allCards[i + 2].GetCardWeight;

                    if (equal)
                    {
                        if (totalWeight >= weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            ret.Add(allCards[i + 2]);
                            break;
                        }
                    }
                    else
                    {
                        if (totalWeight > weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            ret.Add(allCards[i + 2]);
                            break;
                        }
                    }

                }
            }
        }

        return ret;
    }

    /// <summary>
    /// 找到手牌中符合要求的是连子
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    protected List<Card> FindStraight(List<Card> allCards, int minWeight, int length, bool equal)
    {
        List<Card> ret = new List<Card>();
        int counter = 1;
        List<int> indeies = new List<int>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 4)
            {
                int weight = (int)allCards[i].GetCardWeight;
                if (equal)
                {
                    if (weight >= minWeight)
                    {
                        counter = 1;
                        indeies.Clear();

                        for (int j = i + 1; j < allCards.Count; j++)
                        {
                            if (allCards[j].GetCardWeight > Weight.One)
                                break;

                            if ((int)allCards[j].GetCardWeight - weight == counter)
                            {
                                counter++;
                                indeies.Add(j);
                            }

                            if (counter == length)
                                break;
                        }
                    }
                }
                else
                {
                    if (weight > minWeight)
                    {
                        counter = 1;
                        indeies.Clear();

                        for (int j = i + 1; j < allCards.Count; j++)
                        {
                            if (allCards[j].GetCardWeight > Weight.One)
                                break;
                            if ((int)allCards[j].GetCardWeight - weight == counter)
                            {
                                counter++;
                                indeies.Add(j);
                            }

                            if (counter == length)
                                break;
                        }
                    }
                }

            }
            if (counter == length)
            {
                indeies.Insert(0, i);
                break;
            }

        }

        if (counter == length)
        {
            for (int i = 0; i < indeies.Count; i++)
            {
                ret.Add(allCards[indeies[i]]);
            }
        }

        return ret;
    }

    /// <summary>
    /// 找到手牌中符合要求的是双连子
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    protected List<Card> FindDoubleStraight(List<Card> allCards, int minWeight, int length)
    {
        List<Card> ret = new List<Card>();
        int counter = 0;
        List<int> indeies = new List<int>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 4)
            {
                int weight = (int)allCards[i].GetCardWeight;
                if (weight > minWeight)
                {
                    counter = 0;
                    indeies.Clear();

                    int circle = 0;
                    for (int j = i + 1; j < allCards.Count; j++)
                    {
                        if (allCards[j].GetCardWeight > Weight.One)
                            break;

                        if ((int)allCards[j].GetCardWeight - weight == counter)
                        {
                            circle++;
                            if (circle % 2 == 1)
                            {
                                counter++;
                            }
                            indeies.Add(j);
                        }

                        if (counter == length / 2)
                            break;
                    }
                }
            }
            if (counter == length / 2)
            {
                indeies.Insert(0, i);
                break;
            }

        }

        if (counter == length / 2)
        {
            for (int i = 0; i < indeies.Count; i++)
            {
                ret.Add(allCards[indeies[i]]);
            }
        }

        return ret;
    }


    /// <summary>
    /// 三代二
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="weight"></param>
    /// <param name="equal"></param>
    /// <returns></returns>
    protected List<Card> FindThreeAndTwo(List<Card> allCards, int weight, bool equal)
    {
        List<Card> three = FindOnlyThree(allCards, weight, equal);
        if (three.Count != 0)
        {
            List<Card> leftCards = GetAllCards(three);
            List<Card> two = FindDouble(leftCards, (int)Weight.Three, true);

            three.AddRange(two);
        }
        else
            three.Clear();

        return three;

    }

    /// <summary>
    /// 三带一
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="weight"></param>
    /// <param name="equal"></param>
    /// <returns></returns>
    protected List<Card> FindThreeAndOne(List<Card> allCards, int weight, bool equal)
    {
        List<Card> three = FindOnlyThree(allCards, weight, equal);
        if (three.Count != 0)
        {
            List<Card> leftCards = GetAllCards(three);
            List<Card> one = FindSingle(leftCards, (int)Weight.Three, true);
            three.AddRange(one);
        }
        else
            three.Clear();

        return three;
    }

    /// <summary>
    /// Pass label
    /// </summary>
    protected void ShowNotice()
    {
        computerNotice.SetActive(true);
        computerNotice.GetComponent<TweenAlpha>().ResetToBeginning();
        computerNotice.GetComponent<TweenAlpha>().PlayForward();
        StartCoroutine(DisActiveNotice(computerNotice));
    }

    protected IEnumerator DisActiveNotice(GameObject notice)
    {
        yield return new WaitForSeconds(2.0f);
        computerNotice.SetActive(false);
    }
}
