using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelDefinition[] levels;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private UIController uiController;
    private int activeProjectiles = 0;
    private bool waitingForChainReaction = false;
    private bool loseCheckRunning = false;

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
    public void RegisterProjectile()
    {
        activeProjectiles++;
        waitingForChainReaction = true;
    }

    public void UnregisterProjectile()
    {
        activeProjectiles--;
        if (activeProjectiles < 0)
            activeProjectiles = 0;

        if (activeProjectiles == 0)
            StartCoroutine(CheckEndOfChain());
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
        if (RemainingTaps == 0 && !loseCheckRunning)
        {
            StartCoroutine(CheckLoseAfterDelay());
        }

        return true;
    }
    private IEnumerator CheckLoseAfterDelay()
    {
        loseCheckRunning = true;

        // wait for chain reaction to finish visually
        yield return new WaitForSeconds(1.0f); // tweak 0.5–1.5f depending on your speed

        // If board is cleared in that time, win will already trigger, so we don't lose
        if (!boardManager.AllPoppersCleared())
        {
            OnNoMovesLeft();
        }

        loseCheckRunning = false;
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
        if (boardManager.AllPoppersCleared()) return;
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
        boardManager.ClearBoard();
        LoadLevel(CurrentLevelIndex);

    }

    public void LoadNextLevel()
    {
        int next = (CurrentLevelIndex + 1) % levels.Length;
        boardManager.ClearBoard();
        LoadLevel(next);
    }

    public void StartChainReaction()
    {
        waitingForChainReaction = true;
    }
    private IEnumerator CheckEndOfChain()
    {
        // allow final explosion animations to finish
        yield return new WaitForSeconds(0.25f);

        waitingForChainReaction = false;

        // WIN check
        if (boardManager.AllPoppersCleared())
        {
            OnBoardCleared();
            yield break;
        }

        // LOSE check: no taps left & poppers still on board
        if (RemainingTaps <= 0)
        {
            OnNoMovesLeft();
        }
    }



}
