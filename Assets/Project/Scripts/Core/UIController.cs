using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI tapCountText;
    [SerializeField] private TextMeshProUGUI levelLabelText;
    [SerializeField] private TextMeshProUGUI messageText;   // optional (e.g. "Level Complete!")

    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject hudPanel;           // main HUD with taps/level

    [Header("Buttons")]
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButtonOnWin;
    [SerializeField] private Button restartButtonOnLose;
    [SerializeField] private Button quitButton;
    // optional

    [SerializeField] private Button quitLossButton;
    private void Awake()
    {
        // Make sure panels start hidden
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        // HUD visible at start
        if (hudPanel != null) hudPanel.SetActive(true);

        // Wire up button events
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);

        if (restartButtonOnWin != null)
            restartButtonOnWin.onClick.AddListener(OnRestartClicked);

        if (restartButtonOnLose != null)
            restartButtonOnLose.onClick.AddListener(OnRestartClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
        if (quitLossButton != null)
            quitLossButton.onClick.AddListener(OnQuitClicked);
    }

    #region Public API called from GameManager

    public void UpdateTapCount(int taps)
    {
        if (tapCountText != null)
            tapCountText.text = $"Taps: {taps}";
    }

    public void UpdateLevelLabel(int levelNumber)
    {
        if (levelLabelText != null)
            levelLabelText.text = $"Level {levelNumber}";
    }

    public void ShowWinPanel()
    {
        if (hudPanel != null) hudPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        if (messageText != null)
            messageText.text = "Level Complete!";

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    public void ShowLosePanel()
    {
        if (hudPanel != null) hudPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        if (messageText != null)
            messageText.text = "Out of taps!";

        if (losePanel != null)
            losePanel.SetActive(true);
    }

    #endregion

    #region Button Callbacks

    private void OnNextLevelClicked()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(true);

        GameManager.Instance.LoadNextLevel();
    }

    private void OnRestartClicked()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(true);

        GameManager.Instance.RestartLevel();
    }

    private void OnQuitClicked()
    {
        // For editor/game jam: just quit application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion
}
