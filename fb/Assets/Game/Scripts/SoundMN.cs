using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundMN : Singleton<SoundMN>
{
    public AudioSource PlaySfxAudioSource, PlayMusicBGAudioSource;
    [SerializeField] AudioClip backGroundMussic;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip score;

    public float volumeMusic = 0.5f;
    public float volumeSFX = 0.5f;

	private void Start()
	{
        PlayMusic();
    }
	public static void Jump()
    {
        Instance.Play(Instance.jump);
    }
    public static void Dead()
	{
        Instance.Play(Instance.dead);
	}
    public static void Score()
	{
        Instance.Play(Instance.score);
	}

    private void Play(AudioClip audio)
    {
        if (audio)
        {
            PlaySfxAudioSource.clip = audio;
            PlaySfxAudioSource.PlayOneShot(audio, volumeSFX);
        }
    }

    public void PlayMusic()
    {
        PlayMusicBGAudioSource.clip = backGroundMussic;
        PlayMusicBGAudioSource.volume = 0;
        PlayMusicBGAudioSource.playOnAwake = false;
        PlayMusicBGAudioSource.pitch = 1;
        PlayMusicBGAudioSource.loop = true;
        PlayMusicBGAudioSource.Play();
        if (volumeMusic > 0)
            StartCoroutine(FadeAudio(volumeMusic));
    }
    IEnumerator FadeAudio(float target)
    {
        while (PlayMusicBGAudioSource.volume != target)
        {
            PlayMusicBGAudioSource.volume = Mathf.MoveTowards(PlayMusicBGAudioSource.volume, target, Time.deltaTime);
            yield return null;
        }
    }
}
