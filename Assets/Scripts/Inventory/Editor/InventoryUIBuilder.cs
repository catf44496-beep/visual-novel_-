using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor
{
    public class InventoryUIBuilder
    {
        [MenuItem("Tools/Create Inventory UI")]
        public static void BuildUI()
        {
            Canvas canvas = Object.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("当前场景中找不到 Canvas 对象！");
                return;
            }

            // 1. 创建主面板 (对应 HTML .gp)
            GameObject panelObj = CreateUIObject("InventoryPanel", canvas.transform);
            RectTransform panelRT = panelObj.GetComponent<RectTransform>();
            SetRect(panelRT, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(720, 480));
            Image panelBg = panelObj.AddComponent<Image>();
            panelBg.color = GetColor("#1C1B18");
            Outline panelOut = panelObj.AddComponent<Outline>();
            panelOut.effectColor = GetColor("#4A4535");

            // 2. 创建顶部 Header (对应 HTML .gh)
            GameObject headerObj = CreateUIObject("Header", panelObj.transform);
            SetRect(headerObj.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), new Vector2(0, 0), new Vector2(0, 36));
            Image headerBg = headerObj.AddComponent<Image>();
            headerBg.color = GetColor("#222118");

            Text titleTxt = CreateText("TitleText", headerObj.transform, "[ 背包系统 v1.0 ]", 14, "#C8B870");
            SetRect(titleTxt.GetComponent<RectTransform>(), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(70, 0), new Vector2(120, 36));

            Text slotCountTxt = CreateText("SlotCount", headerObj.transform, "15 / 20 格", 12, "#5A5440");
            SetRect(slotCountTxt.GetComponent<RectTransform>(), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(-50, 0), new Vector2(80, 36));
            slotCountTxt.alignment = TextAnchor.MiddleRight;

            // 分割线
            CreateLine("Line1", panelObj.transform, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -36));

            // 3. 创建 TabBar (对应 HTML .tabs)
            GameObject tabBarObj = CreateUIObject("TabBar", panelObj.transform);
            SetRect(tabBarObj.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), new Vector2(0, -37), new Vector2(0, 32));
            HorizontalLayoutGroup hlg = tabBarObj.AddComponent<HorizontalLayoutGroup>();
            hlg.childControlWidth = false;
            hlg.childControlHeight = true;

            Button tab1 = CreateTabButton("Tab_Survival", tabBarObj.transform, "生存物资", true);
            Button tab2 = CreateTabButton("Tab_Codex", tabBarObj.transform, "时代图鉴", false);
            Button tab3 = CreateTabButton("Tab_Market", tabBarObj.transform, "黑市兑换", false);

            CreateLine("Line2", panelObj.transform, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -69));

            // 4. 创建主体容器 (对应 HTML .body)
            GameObject bodyObj = CreateUIObject("Body", panelObj.transform);
            SetRect(bodyObj.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f), new Vector2(0, -20), new Vector2(0, -100));

            // 4.1 左侧 Grid Container
            GameObject gridObj = CreateUIObject("GridContainer", bodyObj.transform);
            SetRect(gridObj.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(0.55f, 1), new Vector2(0, 0.5f), Vector2.zero, Vector2.zero);

            GameObject contentObj = CreateUIObject("Content", gridObj.transform);
            SetRect(contentObj.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            GridLayoutGroup gridLayout = contentObj.AddComponent<GridLayoutGroup>();
            gridLayout.cellSize = new Vector2(64, 64);
            gridLayout.spacing = new Vector2(8, 8);
            gridLayout.padding = new RectOffset(16, 16, 16, 16);
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 5;

            // 4.2 右侧 Detail Panel
            GameObject detailObj = CreateUIObject("DetailPanel", bodyObj.transform);
            SetRect(detailObj.GetComponent<RectTransform>(), new Vector2(0.55f, 0), new Vector2(1, 1), new Vector2(1, 0.5f), Vector2.zero, Vector2.zero);

            // 左边框线
            CreateLine("LeftBorder", detailObj.transform, new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0));

            Text nameTxt = CreateText("NameText", detailObj.transform, "大米", 20, "#D8C870");
            SetRect(nameTxt.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1), new Vector2(140, -30), new Vector2(-32, 30));
            nameTxt.alignment = TextAnchor.MiddleLeft;

            Text rarityTxt = CreateText("RarityText", detailObj.transform, "普通", 12, "#9A9480");
            SetRect(rarityTxt.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1), new Vector2(140, -55), new Vector2(-32, 20));
            rarityTxt.alignment = TextAnchor.MiddleLeft;

            Text descTxt = CreateText("DescText", detailObj.transform, "5kg袋装大米，维持基本热量供应。", 14, "#8A7E65");
            SetRect(descTxt.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1), new Vector2(140, -110), new Vector2(-40, 60));
            descTxt.alignment = TextAnchor.UpperLeft;

            // Effect Box
            GameObject effectBox = CreateUIObject("EffectBox", detailObj.transform);
            SetRect(effectBox.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1), new Vector2(140, -170), new Vector2(-40, 30));
            Image effectBg = effectBox.AddComponent<Image>();
            effectBg.color = GetColor("#1E221E");
            Outline effectOut = effectBox.AddComponent<Outline>();
            effectOut.effectColor = GetColor("#2A3A2A");

            Text effectTxt = CreateText("EffectText", effectBox.transform, "+20 饱食度", 12, "#6AB87A");
            SetStretch(effectTxt.GetComponent<RectTransform>());

            Text weightTxt = CreateText("WeightText", detailObj.transform, "重量 2 kg  |  数量 3", 12, "#5A5440");
            SetRect(weightTxt.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1), new Vector2(140, -210), new Vector2(-40, 20));
            weightTxt.alignment = TextAnchor.MiddleLeft;

            // 5. 创建底部 StatusBar (对应 HTML .sb)
            CreateLine("Line3", panelObj.transform, new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 30));

            GameObject statusObj = CreateUIObject("StatusBar", panelObj.transform);
            SetRect(statusObj.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 0), new Vector2(0, 15), new Vector2(0, 30));
            Image statusBg = statusObj.AddComponent<Image>();
            statusBg.color = GetColor("#181714");

            HorizontalLayoutGroup statusHLG = statusObj.AddComponent<HorizontalLayoutGroup>();
            statusHLG.padding = new RectOffset(14, 14, 0, 0);
            statusHLG.spacing = 20;
            statusHLG.childControlWidth = false;
            statusHLG.childControlHeight = true;
            statusHLG.childAlignment = TextAnchor.MiddleLeft;

            CreateStatusItem("StatWeight", statusObj.transform, "总重量", "7.2 kg", "#8B7A3A");
            CreateStatusItem("StatCodex", statusObj.transform, "图鉴", "4/6", "#8B7A3A");
            CreateStatusItem("StatRep", statusObj.transform, "信誉", "良好", "#6AB87A");

            // 6. 创建 CloseButton
            Button closeBtn = CreateTabButton("CloseButton", headerObj.transform, "X", false);
            SetRect(closeBtn.GetComponent<RectTransform>(), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(0, 0), new Vector2(36, 36));
            UnityEngine.Events.UnityAction<bool> closeAction = new UnityEngine.Events.UnityAction<bool>(panelObj.SetActive);
            UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(closeBtn.onClick, closeAction, false);

            // ==========================================
            // 7. 创建并保存 InventorySlot Prefab
            // ==========================================
            GameObject slotObj = CreateUIObject("InventorySlot", null);
            SetRect(slotObj.GetComponent<RectTransform>(), Vector2.zero, Vector2.zero, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(64, 64));
            Image slotBg = slotObj.AddComponent<Image>();
            slotBg.color = GetColor("#252318");
            Outline slotOut = slotObj.AddComponent<Outline>();
            slotOut.effectColor = GetColor("#3A3628");

            Image rarityPip = CreateUIObject("RarityPip", slotObj.transform).AddComponent<Image>();
            SetRect(rarityPip.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(4, -4), new Vector2(8, 8));
            rarityPip.color = GetColor("#9A9480");

            // 内部图标背景和文字(模拟HTML版)
            Image iconBg = CreateUIObject("IconBG", slotObj.transform).AddComponent<Image>();
            SetRect(iconBg.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(44, 44));
            iconBg.color = GetColor("#252318");

            Text iconTxt = CreateText("IconText", iconBg.transform, "空", 14, "#E8DDB0");
            SetStretch(iconTxt.GetComponent<RectTransform>());

            Text countTxt = CreateText("CountText", slotObj.transform, "1", 12, "#8B7A3A");
            SetRect(countTxt.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f), new Vector2(-4, 2), Vector2.zero);
            countTxt.alignment = TextAnchor.LowerRight;

            Image selectedOverlay = CreateUIObject("SelectedOverlay", slotObj.transform).AddComponent<Image>();
            SetStretch(selectedOverlay.GetComponent<RectTransform>());
            selectedOverlay.color = new Color(0, 0, 0, 0); // 透明
            Outline selOut = selectedOverlay.gameObject.AddComponent<Outline>();
            selOut.effectColor = GetColor("#D8C870");
            selectedOverlay.gameObject.SetActive(false);

            // 保存 Prefab
            if (!System.IO.Directory.Exists("Assets/Prefabs"))
                System.IO.Directory.CreateDirectory("Assets/Prefabs");

            string prefabPath = "Assets/Prefabs/" + "InventorySlot.prefab";
            GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(slotObj, prefabPath);
            Object.DestroyImmediate(slotObj);

            // ==========================================
            // 8. 填充测试物品数据 (还原网页版效果)
            // ==========================================
            string[] mockNames = { "大米", "罐头", "药剂", "工具", "净水", "打火", "急救", "信号" };
            string[] mockBGs = { "#233023", "#22223A", "#3A223A", "#3A3822", "#1A2A3A", "#3A2A1A", "#3A2020", "#3A1A1A" };
            string[] mockRarities = { "#9A9480", "#9A9480", "#7A9ACA", "#7AB87A", "#9A9480", "#9A9480", "#7AB87A", "#7A9ACA" };
            string[] mockCounts = { "3", "7", "2", "1", "5", "4", "1", "1" };

            for (int i = 0; i < 20; i++)
            {
                GameObject inst = (GameObject)PrefabUtility.InstantiatePrefab(savedPrefab, contentObj.transform);
                if (i < mockNames.Length)
                {
                    inst.transform.Find("IconBG").GetComponent<Image>().color = GetColor(mockBGs[i]);
                    inst.transform.Find("IconBG/IconText").GetComponent<Text>().text = mockNames[i];
                    inst.transform.Find("CountText").GetComponent<Text>().text = mockCounts[i];
                    inst.transform.Find("RarityPip").GetComponent<Image>().color = GetColor(mockRarities[i]);

                    if (i == 0) // 默认选中第一个
                        inst.transform.Find("SelectedOverlay").gameObject.SetActive(true);
                }
                else
                {
                    // 空槽位表现
                    inst.transform.Find("IconBG").gameObject.SetActive(false);
                    inst.transform.Find("CountText").gameObject.SetActive(false);
                    inst.transform.Find("RarityPip").gameObject.SetActive(false);
                    inst.GetComponent<Image>().color = new Color(0.14f, 0.13f, 0.09f, 0.5f);
                }
            }

            // 完成设置
            Undo.RegisterCreatedObjectUndo(panelObj, "Create Inventory UI");
            Debug.Log("废土风格 Inventory UI 构建成功！已自动填充测试预览数据。");
        }

        // ========================== 工具函数 ==========================

        static GameObject CreateUIObject(string name, Transform parent)
        {
            GameObject go = new GameObject(name);
            if (parent != null) go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            return go;
        }

        static Text CreateText(string name, Transform parent, string content, int fontSize, string hexColor)
        {
            GameObject go = CreateUIObject(name, parent);
            Text txt = go.AddComponent<Text>();
            txt.text = content;
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.fontSize = fontSize;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = GetColor(hexColor);
            return txt;
        }

        static Button CreateTabButton(string name, Transform parent, string textContent, bool isSelected)
        {
            GameObject go = CreateUIObject(name, parent);
            SetRect(go.GetComponent<RectTransform>(), Vector2.zero, Vector2.zero, new Vector2(0, 1), Vector2.zero, new Vector2(100, 32));
            Image img = go.AddComponent<Image>();
            img.color = GetColor(isSelected ? "#2A2818" : "#00000000");
            Button btn = go.AddComponent<Button>();

            Text txt = CreateText("Text", go.transform, textContent, 14, isSelected ? "#D8C870" : "#5A5440");
            SetStretch(txt.GetComponent<RectTransform>());

            // 添加右侧分割线
            CreateLine("RightLine", go.transform, new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0));

            return btn;
        }

        static void CreateStatusItem(string name, Transform parent, string title, string val, string valHex)
        {
            GameObject go = CreateUIObject(name, parent);
            SetRect(go.GetComponent<RectTransform>(), Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, new Vector2(100, 30));
            HorizontalLayoutGroup hlg = go.AddComponent<HorizontalLayoutGroup>();
            hlg.childControlWidth = true; hlg.childControlHeight = true;

            Text tTxt = CreateText("Title", go.transform, title, 12, "#5A5440");
            Text vTxt = CreateText("Value", go.transform, " " + val, 12, valHex);
            tTxt.alignment = TextAnchor.MiddleLeft;
            vTxt.alignment = TextAnchor.MiddleLeft;
        }

        static void CreateLine(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size = default)
        {
            GameObject line = CreateUIObject(name, parent);
            if (size == default) size = new Vector2(0, 1);
            SetRect(line.GetComponent<RectTransform>(), anchorMin, anchorMax, new Vector2(0.5f, 0.5f), pos, size);
            line.AddComponent<Image>().color = GetColor("#4A4535");
        }

        static void SetStretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }

        static void SetRect(RectTransform rt, Vector2 aMin, Vector2 aMax, Vector2 pivot, Vector2 pos, Vector2 size)
        {
            rt.anchorMin = aMin; rt.anchorMax = aMax; rt.pivot = pivot;
            rt.anchoredPosition = pos; rt.sizeDelta = size;
        }

        static Color GetColor(string hex)
        {
            Color color;
            ColorUtility.TryParseHtmlString(hex, out color);
            return color;
        }
    }
}