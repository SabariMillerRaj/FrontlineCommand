using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    public AudioClip bgmClip;
    [Range(0f, 1f)] public float bgmVolume = 0.4f;

    [Header("SFX")]
    public AudioClip gunShotSFX;
    public AudioClip explosionSFX;
    public AudioClip waveAlertSFX;
    public AudioClip gameOverSFX;
    public AudioClip placementSFX;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.clip = bgmClip;

        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        if (bgmClip != null) bgmSource.Play();

        GameManager.OnWaveStart += PlayWaveAlert;
        GameManager.OnGameOver += PlayGameOver;
    }

    void OnDestroy()
    {
        GameManager.OnWaveStart -= PlayWaveAlert;
        GameManager.OnGameOver -= PlayGameOver;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayGunShot() => PlaySFX(gunShotSFX);
    public void PlayExplosion() => PlaySFX(explosionSFX);
    public void PlayPlacement() => PlaySFX(placementSFX);
    void PlayWaveAlert() => PlaySFX(waveAlertSFX);
    void PlayGameOver() => PlaySFX(gameOverSFX);
}
