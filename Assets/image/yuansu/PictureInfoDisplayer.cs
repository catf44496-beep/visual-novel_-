using UnityEngine;
using UnityEngine.UI;

public class PictureInfoDisplayer : MonoBehaviour
{
    public GameObject infoPanel;         // 信息面板（UI Panel）
    public Text titleText;               // 标题文本组件
    public Text descriptionText;         // 描述文本组件

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            // 将鼠标位置转换为世界坐标
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 发射2D射线检测
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                // 获取被点击物体上的 PictureInfo 脚本
                PictureInfo pictureInfo = hit.collider.GetComponent<PictureInfo>();
                if (pictureInfo != null)
                {
                    // 更新UI内容
                    titleText.text = pictureInfo.title;
                    descriptionText.text = pictureInfo.description;
                    // 显示信息面板
                    infoPanel.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // 右键点击逻辑
        }
    }

    // 可选：关闭信息面板的方法
    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}