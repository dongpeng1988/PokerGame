using UnityEngine;
using System.Collections;

/// <summary>
/// 重新开始游戏面板
/// </summary>
public class Restart : MonoBehaviour
{
    private GameController controller;
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject clickButton = transform.Find("Button").gameObject;
        clickButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(RestartGame));
    }

    /// <summary>
    /// 面板定时消失
    /// </summary>
    /// <param name="sec"></param>
    public void SetTimeToNext(float sec)
    {
        Invoke("Next", sec);
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    void RestartGame()
    {
        //先清理所有卡牌
        controller.BackToDeck();
        controller.DestroyAllSprites();
        DeskCardsCache.Instance.Clear();

        Destroy(GameObject.Find("InteractionPanel").gameObject);
        Destroy(GameObject.Find("ScenePanel").gameObject);
        Destroy(GameObject.Find("BackgroundPanel").gameObject);
        Destroy(this.gameObject);

        //重置事件，防止引用錯務
        OrderController.Instance.ResetButton();
        OrderController.Instance.ResetSmartCard();

        GameObject panel = NGUITools.AddChild(UICamera.mainCamera.gameObject, (GameObject)Resources.Load("StartPanel"));
        panel.AddComponent<Menu>();
        panel.transform.Find("NoticeLabel").gameObject.SetActive(true);
    }

    /// <summary>
    /// 下一场
    /// </summary>
    void Next()
    {
        controller.BackToDeck();
        controller.DestroyAllSprites();
        DeskCardsCache.Instance.Clear();
        GameObject deal = GameObject.Find("InteractionPanel").transform.Find("DealBtn").gameObject;
        deal.SetActive(true);
        Destroy(this.gameObject);
        ResetDisplay();
    }

    /// <summary>
    /// 重置玩家显示
    /// </summary>
    void ResetDisplay()
    {
        for (int i = 1; i < 4; i++)
        {
            if (GameObject.Find(((CharacterType)i).ToString()))
            {
                GameController.UpdateLeftCardsCount((CharacterType)i, 0);
                GameController.UpdateIndentity((CharacterType)i, Identity.Farmer);
            }

        }
    }
}
