using UnityEngine;
using Fungus;

public class FungusSceneAnimation : MonoBehaviour
{
    public Flowchart flowchart;      // 在 Inspector 中指定 Flowchart
    public Animator animator;        // 在 Inspector 中指定 Animator

    void Start()
    {
        // 确保场景加载完成后立即执行
        if (flowchart != null)
        {
            // 触发名为 "PlayAnimation" 的 Block
            flowchart.ExecuteBlock("PlayAnimation");
        }
    }
}