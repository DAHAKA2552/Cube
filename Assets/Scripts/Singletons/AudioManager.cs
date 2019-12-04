using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    #region Nested Types

    [System.Serializable]
    struct Clip
    {
        public ClipName name;
        public AudioClip clip;

        public Clip(ClipName n, AudioClip c)
        {
            name = n;
            clip = c;
        }
    }

    #endregion



    #region Fields

    public static AudioManager instance = null;

    [SerializeField] AudioSource regularSfxSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] float sfxVolume = 0.1f;
    [SerializeField] float musicVolume = 0.1f;
    [SerializeField] Clip[] clips;

    bool isEnabled = true;

    #endregion



    #region Unity Lifecycle

    void Start()
    {
        sfxSource.volume = sfxVolume;
        regularSfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;

        //PlayMusic();
    }

    #endregion



    #region Public Methods

    public void SwitchSoundPermission()
    {
        if (isEnabled == true)
        {
            musicSource.Pause();
            isEnabled = false;
        }
        else
        {
            isEnabled = true;
            UnPauseMusic();
        }
    }


    public void PlaySFXClip(ClipName clipName)
    {
        if (isEnabled)
        {
            foreach (Clip c in clips)
            {
                if (c.name == clipName)
                {
                    sfxSource.clip = c.clip;
                }
            }

            sfxSource.Play();
        }
    }


    public void PlayRegularSFXClip(ClipName clipName)
    {
        if (isEnabled)
        {
            foreach (Clip c in clips)
            {
                if (c.name == clipName)
                {
                    regularSfxSource.clip = c.clip;
                }
            }

            regularSfxSource.Play();
        }
    }


    public void PlayMusic()
    {
        if (isEnabled)
        {
            musicSource.Play();
        }
    }


    public void PauseMusic()
    {
        musicSource.Pause();
    }


    public void UnPauseMusic()
    {
        if (isEnabled)
        {
            musicSource.UnPause();
        }
    }


    public void StopPlayingSFX()
    {
        sfxSource.Stop();
    }


    public void StopPlayingMusic()
    {
        musicSource.Stop();
    }

    #endregion
}
