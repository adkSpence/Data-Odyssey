using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ARMenuManager : MonoBehaviour
{
    private Canvas arCanvas;
    private RectTransform canvasRect;

    void Start()
    {
        SetupARCanvas();
        CreateWelcomeText();
        CreateGraphButtons();
    }

    private void SetupARCanvas()
    {
        GameObject canvasObj = new GameObject("AR Menu Canvas");
        arCanvas = canvasObj.AddComponent<Canvas>();
        arCanvas.renderMode = RenderMode.ScreenSpaceOverlay; // Changed to overlay

        // Add canvas scaler for proper UI scaling across devices
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920); // Standard mobile resolution
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1f; // Match height for vertical layout

        canvasObj.AddComponent<GraphicRaycaster>();
        canvasRect = arCanvas.GetComponent<RectTransform>();
    }

    private void CreateWelcomeText()
    {
        GameObject textObj = new GameObject("Welcome Text");
        textObj.transform.SetParent(arCanvas.transform, false);

        TextMeshProUGUI welcomeText = textObj.AddComponent<TextMeshProUGUI>();
        welcomeText.text = "Welcome to Data Odyssey";
        welcomeText.fontSize = 48;
        welcomeText.color = Color.white;
        welcomeText.alignment = TextAlignmentOptions.Center;

        RectTransform textRect = welcomeText.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 1);
        textRect.anchorMax = new Vector2(1, 1);
        textRect.pivot = new Vector2(0.5f, 1);
        textRect.anchoredPosition = new Vector2(0, -100);
        textRect.sizeDelta = new Vector2(0, 100);
    }

    private void CreateGraphButtons()
    {
        (string buttonText, string sceneName)[] graphs = new[]
        {
            ("Bar Chart", "BarGraphScene"),
            ("HeatMap Chart", "HeatmapScene"),
            ("Back to Main", "MenuScene")
        };

        // Create a panel for buttons
        GameObject panelObj = new GameObject("Button Panel");
        panelObj.transform.SetParent(arCanvas.transform, false);

        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = new Vector2(0, 0);
        panelRect.sizeDelta = new Vector2(300, 400);

        float buttonSpacing = 100f;

        for (int i = 0; i < graphs.Length; i++)
        {
            CreateButton(graphs[i].buttonText, graphs[i].sceneName,
                        new Vector2(0, 150 - (buttonSpacing * i)),
                        panelObj.transform);
        }
    }

    private void CreateButton(string buttonText, string sceneName, Vector2 position, Transform parent)
    {
        GameObject buttonObj = new GameObject(buttonText + " Button");
        buttonObj.transform.SetParent(parent, false);

        Button button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();

        // Add background image
        buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        TextMeshProUGUI buttonTMP = textObj.AddComponent<TextMeshProUGUI>();
        buttonTMP.text = buttonText;
        buttonTMP.fontSize = 24;
        buttonTMP.color = Color.white;
        buttonTMP.alignment = TextAlignmentOptions.Center;

        RectTransform buttonRect = button.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = position;
        buttonRect.sizeDelta = new Vector2(250, 60);

        RectTransform textRect = buttonTMP.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 0.9f);
        colors.pressedColor = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        button.colors = colors;

        button.onClick.AddListener(() => LoadGraphScene(sceneName));
    }

    private void LoadGraphScene(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}