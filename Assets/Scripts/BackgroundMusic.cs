using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // Аудио клип для фоновой музыки
    public float volume = 0.5f; // Громкость фоновой музыки
    public bool loop = true; // Повторение музыки

    private AudioSource audioSource;
    private static BackgroundMusic instance;

    void Awake()
    {
        // Проверка на существование экземпляра
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Уничтожаем новый объект, если экземпляр уже существует
            return;
        }

        // Устанавливаем текущий экземпляр
        instance = this;
        DontDestroyOnLoad(gameObject); // Предотвращаем уничтожение объекта при загрузке новой сцены

        // Получаем компонент AudioSource и настраиваем его
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.volume = volume;
        audioSource.loop = loop;
    }

    void Start()
    {
        // Воспроизводим фоновую музыку
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}