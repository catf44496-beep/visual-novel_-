using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public GameObject currentCanvas; // 当前显示的Canvas
    public GameObject nextCanvas;    // 需要跳转到的目标Canvas
    public GameObject thirdCanvas;   // 第三个Canvas
    public GameObject fourthCanvas;  // 第四个Canvas
    public GameObject fifthCanvas;   // 第五个Canvas
    public GameObject sixthCanvas;   // 第六个Canvas
    public GameObject seventhCanvas; // 第七个Canvas
    public GameObject eighthCanvas;  // 第八个Canvas

    // 切换到下一个 Canvas
    public void SwitchToNextCanvas()
    {
        if (currentCanvas != null)
            currentCanvas.SetActive(false);

        if (nextCanvas != null)
        {
            nextCanvas.SetActive(true);
            currentCanvas = nextCanvas; // 更新当前Canvas引用
        }
    }

    /// <summary>
    /// 切换到指定的 Canvas。
    /// </summary>
    /// <param name="targetCanvas">目标 Canvas 对象。</param>
    public void SwitchToCanvas(GameObject targetCanvas)
    {
        if (currentCanvas != null)
            currentCanvas.SetActive(false);

        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true);
            currentCanvas = targetCanvas;
        }
    }

    // 跳转到第三个 Canvas
    public void SwitchToThirdCanvas()
    {
        Debug.Log("SwitchToThirdCanvas called");
        if (thirdCanvas == null)
        {
            Debug.LogError("thirdCanvas is not assigned!");
            return;
        }
        SwitchToCanvas(thirdCanvas);
    }

    // 跳转到第四个 Canvas
    public void SwitchToFourthCanvas()
    {
        Debug.Log("SwitchToFourthCanvas called");
        if (fourthCanvas == null)
        {
            Debug.LogError("fourthCanvas is not assigned!");
            return;
        }
        SwitchToCanvas(fourthCanvas);
    }

    // 跳转到第五个 Canvas
    public void SwitchToFifthCanvas()
    {
        Debug.Log("SwitchToFifthCanvas called");
        if (fifthCanvas == null)
        {
            Debug.LogError("fifthCanvas is not assigned!");
            return;
        }
        SwitchToCanvas(fifthCanvas);
    }

    // 跳转到第六个 Canvas
    public void SwitchToSixthCanvas()
    {
        Debug.Log("SwitchToSixthCanvas called");
        if (sixthCanvas == null)
        {
            Debug.LogError("sixthCanvas is not assigned!");
            return;
        }
        SwitchToCanvas(sixthCanvas);
    }

    // 跳转到第七个 Canvas
    public void SwitchToSeventhCanvas()
    {
        Debug.Log("SwitchToSeventhCanvas called");
        if (seventhCanvas == null)
        {
            Debug.LogError("seventhCanvas is not assigned!");
            return;
        }
        SwitchToCanvas(seventhCanvas);
    }

    // 跳转到第八个 Canvas
    public void SwitchToEighthCanvas()
    {
        Debug.Log("SwitchToEighthCanvas called");
        if (eighthCanvas == null)
        {
            Debug.LogError("eighthCanvas is not assigned!");
            return;
        }
        SwitchToCanvas(eighthCanvas);
    }

    // 跳转到指定场景
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 从Fungus切换Canvas
    public void SwitchCanvasFromFungus(GameObject targetCanvas)
    {
        SwitchToCanvas(targetCanvas);
    }
}