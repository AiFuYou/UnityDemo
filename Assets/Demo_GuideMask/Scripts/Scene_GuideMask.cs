using UnityEngine;
using UnityEngine.UI;

public class Scene_GuideMask : MonoBehaviour
{
    public Button btn1;
    public Button btn2;
    
    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(OnBtn1Clk);
        btn2.onClick.AddListener(OnBtn2Clk);
    }

    void OnBtn1Clk()
    {
        Debug.Log("新手引导功能触发");
    }
    
    void OnBtn2Clk()
    {
        Debug.Log("被屏蔽的按钮功能触发");
    }
}
