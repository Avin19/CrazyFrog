using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip popClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;

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

    public void PlayBackgroundMusic()
    {
        if (musicSource.clip == backgroundMusic && musicSource.isPlaying) return;
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayPop() => sfxSource.PlayOneShot(popClip);
    public void PlayLevelComplete() => sfxSource.PlayOneShot(winClip);
    public void PlayLevelFailed() => sfxSource.PlayOneShot(loseClip);
}
