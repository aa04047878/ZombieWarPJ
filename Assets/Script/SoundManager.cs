using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> dictAudio;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
        dictAudio = new Dictionary<string, AudioClip>();
    }

    private void Start()
    {
      
    }
    
    /// <summary>
    /// 載入音效
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    public AudioClip LoadAudio(string Path)
    {
        return Resources.Load<AudioClip>(Path);
    }

    /// <summary>
    /// 取得音效
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    private AudioClip GetAudio(string Path)
    {
        if (!dictAudio.ContainsKey(Path))
        {
            dictAudio[Path] = LoadAudio(Path);
        }
        return dictAudio[Path];
    }

    /// <summary>
    /// 播放BGM(不可疊加)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void PlayBGM(string name, float value = 0.1f)
    {
        audioSource.Stop();
        audioSource.clip = GetAudio(name);
        audioSource.Play();
        audioSource.volume = value;
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// 播放音效(可疊加)
    /// </summary>
    public void PlaySound(string path, float value = 0.1f)
    {
        //PlayOneShot，多個音效憶起播放
        audioSource.PlayOneShot(GetAudio(path), value);
        audioSource.volume = value;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }
}
