using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Collections;

public class ObjectController : MonoBehaviour
{
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SendMessageToJS(string message);
#endif

    private int[] configs = new int[4];
    private readonly string[] configNames = { "Material", "Texture", "Color", "Normals" };

    // UI References for names and values separately
    public TextMeshProUGUI config0NameText, config0ValueText;
    public TextMeshProUGUI config1NameText, config1ValueText;
    public TextMeshProUGUI config2NameText, config2ValueText;
    public TextMeshProUGUI config3NameText, config3ValueText;

    // UI Button References
    public Button config0BtnPrev, config0BtnNext;
    public Button config1BtnPrev, config1BtnNext;
    public Button config2BtnPrev, config2BtnNext;
    public Button config3BtnPrev, config3BtnNext;

    // QR Panel and Buttons
    public GameObject QRPanel;
    public Button ShowQRBtn, HideQRBtn;
    public TextMeshProUGUI QRConfigCodeText;
    public Image QRImage;

    // Public properties with UI updates
    public int Config0
    {
        get => configs[0];
        set { configs[0] = Mathf.Clamp(value, 0, 3); UpdateUI(); }
    }
    public int Config1
    {
        get => configs[1];
        set { configs[1] = Mathf.Clamp(value, 0, 3); UpdateUI(); }
    }
    public int Config2
    {
        get => configs[2];
        set { configs[2] = Mathf.Clamp(value, 0, 3); UpdateUI(); }
    }
    public int Config3
    {
        get => configs[3];
        set { configs[3] = Mathf.Clamp(value, 0, 3); UpdateUI(); }
    }

    private IEnumerator RequestURLParamsAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // Delay to ensure Unity is loaded

        #if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("Requesting URL Params from JavaScript...");
            SendMessageToJS("RequestURLParams");
        #endif
    }

    private void Start()
    {
        // Ensure QRPanel is disabled at start
        if (QRPanel) QRPanel.SetActive(false);
        
        StartCoroutine(RequestURLParamsAfterDelay()); // Now it waits before calling JS

        // Assign button listeners with null checks
        if (config0BtnPrev) config0BtnPrev.onClick.AddListener(() => ChangeConfig(0, -1));
        if (config0BtnNext) config0BtnNext.onClick.AddListener(() => ChangeConfig(0, 1));
        if (config1BtnPrev) config1BtnPrev.onClick.AddListener(() => ChangeConfig(1, -1));
        if (config1BtnNext) config1BtnNext.onClick.AddListener(() => ChangeConfig(1, 1));
        if (config2BtnPrev) config2BtnPrev.onClick.AddListener(() => ChangeConfig(2, -1));
        if (config2BtnNext) config2BtnNext.onClick.AddListener(() => ChangeConfig(2, 1));
        if (config3BtnPrev) config3BtnPrev.onClick.AddListener(() => ChangeConfig(3, -1));
        if (config3BtnNext) config3BtnNext.onClick.AddListener(() => ChangeConfig(3, 1));
        
        // Assign QR panel button listeners
        if (ShowQRBtn) ShowQRBtn.onClick.AddListener(() => ShowQRPanel());
        if (HideQRBtn) HideQRBtn.onClick.AddListener(() => HideQRPanel());

        UpdateUI(); // Ensure UI updates when game starts
    }

    public void SetConfigFromJS(string configString)
    {
        if (configString.Length == 4)
        {
            Config0 = int.Parse(configString[0].ToString());
            Config1 = int.Parse(configString[1].ToString());
            Config2 = int.Parse(configString[2].ToString());
            Config3 = int.Parse(configString[3].ToString());
            UpdateUI();
        }
    }

    private void ChangeConfig(int index, int direction)
    {
        int oldValue = configs[index];
        configs[index] = Mathf.Clamp(configs[index] + direction, 0, 3);
        Debug.Log($"Config {index} changed from {oldValue} to {configs[index]}");
        UpdateUI();
    }

    private void ShowQRPanel()
    {
        string QRConfigCode = $"{Config0}{Config1}{Config2}{Config3}";
        if (QRConfigCodeText) QRConfigCodeText.text = QRConfigCode;
        
        // Load the corresponding QR image from Resources folder
        Sprite qrSprite = Resources.Load<Sprite>($"QR/{QRConfigCode}");
        if (qrSprite)
        {
            QRImage.sprite = qrSprite;
        }
        else
        {
            Debug.LogError($"QR Image for {QRConfigCode} not found in Assets/Resources/QR/");
        }
        
        if (QRPanel) QRPanel.SetActive(true);
    }

    private void HideQRPanel()
    {
        if (QRPanel) QRPanel.SetActive(false);
    }

    private void UpdateUI()
    {
        if (config0NameText) config0NameText.text = configNames[0];
        if (config0ValueText) config0ValueText.text = Config0.ToString();
        
        if (config1NameText) config1NameText.text = configNames[1];
        if (config1ValueText) config1ValueText.text = Config1.ToString();
        
        if (config2NameText) config2NameText.text = configNames[2];
        if (config2ValueText) config2ValueText.text = Config2.ToString();
        
        if (config3NameText) config3NameText.text = configNames[3];
        if (config3ValueText) config3ValueText.text = Config3.ToString();
    }
}
