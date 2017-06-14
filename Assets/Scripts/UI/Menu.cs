using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

/// <summary>
/// 菜单面板
/// </summary>
public class Menu : MonoBehaviour
{
    private GameController controller;
    // Use this for initialization
    void Start()
    {
        transform.Find("Easy").gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(StartEasyGame));
        transform.Find("Normal").gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(StartNormalGame));
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    /// <summary>
    /// 选择简单模式
    /// </summary>
    void StartEasyGame()
    {
        controller.InitInteraction();
        controller.InitScene();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 选择普通模式
    /// </summary>
    void StartNormalGame()
    {
        //普通场倍数2
        controller.Multiples = 2;
        controller.InitInteraction();
        controller.InitScene();
        Destroy(this.gameObject);
    }
}
