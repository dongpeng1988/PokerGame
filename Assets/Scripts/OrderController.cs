using UnityEngine;
using System.Collections;

public delegate void CardEvent(bool arg);
/// <summary>
/// 出牌顺序权限管理
/// </summary>
public class OrderController
{
    private CharacterType biggest;//最大出牌者
    private CharacterType currentAuthority;//当前出牌者
    private static OrderController instance;
    public event CardEvent smartCard;//电脑智能出牌
    public event CardEvent activeButton;//激活按钮
    public static OrderController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new OrderController();
            }
            return instance;
        }
    }

    /// <summary>
    /// 当前出牌者
    /// </summary>
    public CharacterType Type
    {
        get { return currentAuthority; }
    }

    /// <summary>
    /// 最大出牌者
    /// </summary>
    public CharacterType Biggest
    {
        set { biggest = value; }
        get { return biggest; }
    }

    private OrderController()
    {
        currentAuthority = CharacterType.Desk;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="type"></param>
    public void Init(CharacterType type)
    {
        currentAuthority = type;
        Biggest = type;
        if (currentAuthority == CharacterType.Player)
        {
            //初始为玩家 ， 玩家必须出牌
            activeButton(false);
        }
        else
        {
            //电脑自动出牌
            smartCard(true);
        }
    }

    /// <summary>
    /// 出牌轮转
    /// </summary>
    public void Turn()
    {
        currentAuthority += 1;

        if (currentAuthority == CharacterType.Desk)
        {
            currentAuthority = CharacterType.Player;
        }

        if (currentAuthority == CharacterType.ComputerOne ||
            currentAuthority == CharacterType.ComputerTwo)
        {
            smartCard(biggest == currentAuthority);
        }
        else if (currentAuthority == CharacterType.Player)
        {
            activeButton(biggest != currentAuthority);

        }

    }

    /// <summary>
    /// 重置事件
    /// </summary>
    public void ResetButton()
    {
        activeButton = null;

    }

    public void ResetSmartCard()
    {
        smartCard = null;
    }
}
