using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayCard : MonoBehaviour
{
    /// <summary>
    /// 遍历选中的牌和牌精灵
    /// </summary>
    public bool CheckSelectCards()
    {
        CardSprite[] sprites = this.GetComponentsInChildren<CardSprite>();

        //找出所有选中的牌
        List<Card> selectedCardsList = new List<Card>();
        List<CardSprite> selectedSpriteList = new List<CardSprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].Select)
            {
                selectedSpriteList.Add(sprites[i]);
                selectedCardsList.Add(sprites[i].Poker);
            }
        }
        //排好序
        CardRules.SortCards(selectedCardsList, true);
        //出牌
        return CheckPlayCards(selectedCardsList, selectedSpriteList);
    }

    /// <summary>
    /// 检测玩家出牌
    /// </summary>
    /// <param name="selectedCardsList"></param>
    /// <param name="selectedSpriteList"></param>
    bool CheckPlayCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList)
    {
        GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
        Card[] selectedCardsArray = selectedCardsList.ToArray();
        //检测是否符合出牌规则
        CardsType type;
        if (CardRules.PopEnable(selectedCardsArray, out type))
        {
            CardsType rule = DeskCardsCache.Instance.Rule;
            if (OrderController.Instance.Biggest == OrderController.Instance.Type)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (DeskCardsCache.Instance.Rule == CardsType.None)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            //炸弹
            else if (type == CardsType.Boom && rule != CardsType.Boom)
            {
                controller.Multiples = 2;
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (type == CardsType.JokerBoom)
            {
                controller.Multiples = 4;
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (type == CardsType.Boom && rule == CardsType.Boom &&
               GameController.GetWeight(selectedCardsArray, type) > DeskCardsCache.Instance.TotalWeight)
            {
                controller.Multiples = 2;
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (GameController.GetWeight(selectedCardsArray, type) > DeskCardsCache.Instance.TotalWeight)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 玩家出牌
    /// </summary>
    /// <param name="selectedCardsList"></param>
    /// <param name="selectedSpriteList"></param>
    void PlayCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList, CardsType type)
    {
        HandCards player = GameObject.Find("Player").GetComponent<HandCards>();
        //如果符合将牌从手牌移到出牌缓存区
        DeskCardsCache.Instance.Clear();
        DeskCardsCache.Instance.Rule = type;

        for (int i = 0; i < selectedSpriteList.Count; i++)
        {
            //先进行卡牌移动
            player.PopCard(selectedSpriteList[i].Poker);
            DeskCardsCache.Instance.AddCard(selectedSpriteList[i].Poker);
            selectedSpriteList[i].transform.parent = GameObject.Find("Desk").transform;
        }

        DeskCardsCache.Instance.Sort();
        GameController.AdjustCardSpritsPosition(CharacterType.Desk);
        GameController.AdjustCardSpritsPosition(CharacterType.Player);

        GameController.UpdateLeftCardsCount(CharacterType.Player, player.CardsCount);

        if (player.CardsCount == 0)
            GameObject.Find("GameController").GetComponent<GameController>().GameOver();
        else
        {
            OrderController.Instance.Biggest = CharacterType.Player;
            OrderController.Instance.Turn();
        }
    }
}
