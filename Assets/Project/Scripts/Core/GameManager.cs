using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelDefinition[] levels;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private UIController uiController;

    public int CurrentLevelIndex { get; private set; } = 0;
    public int RemainingTaps { get; private set; }

    private LevelDefinition currentLevel;
    private bool levelFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadLevel(CurrentLevelIndex);
    }

    public void LoadLevel(int index)
    {
        levelFinished = false;
        CurrentLevelIndex = Mathf.Clamp(index, 0, levels.Length - 1);
        currentLevel = levels[CurrentLevelIndex];
        RemainingTaps = currentLevel.maxTaps;

        uiController.UpdateTapCount(RemainingTaps);
        uiController.UpdateLevelLabel(CurrentLevelIndex + 1);

        boardManager.BuildBoard(currentLevel);
        AudioManager.Instance.PlayBackgroundMusic();
    }

    public bool TryUseTap()
    {
        if (levelFinished) return false;
        if (RemainingTaps <= 0) return false;

        RemainingTaps--;
        uiController.UpdateTapCount(RemainingTaps);
        return true;
    }

    public void OnBoardCleared()
    {
        if (levelFinished) return;
        levelFinished = true;
        AudioManager.Instance.PlayLevelComplete();
        uiController.ShowWinPanel();
    }

    public void OnNoMovesLeft()
    {
        if (levelFinished) return;
        if (RemainingTaps == 0 && !boardManager.AllPoppersCleared())
        {
            levelFinished = true;
            AudioManager.Instance.PlayLevelFailed();
            uiController.ShowLosePanel();
        }
    }

    public void RestartLevel()
    {
        boardManager.ClearBoard();
        LoadLevel(CurrentLevelIndex);
    }

    public void LoadNextLevel()
    {
        int next = (CurrentLevelIndex + 1) % levels.Length;
        boardManager.ClearBoard();
        LoadLevel(next);
    }
}
