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
        //���o���ĸ��
        AudioData audioData = LocalConfig.LoadAudioData();
        SetVolume(audioData.bgmVolume);
    }
    
    /// <summary>
    /// ���J����
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    public AudioClip LoadAudio(string Path)
    {
        return Resources.Load<AudioClip>(Path);
    }

    /// <summary>
    /// ���o����
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
    /// ����BGM(���i�|�[)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void PlayBGM(string name)
    {
        float volume = GetVolume();
        audioSource.Stop();
        audioSource.clip = GetAudio(name);
        audioSource.Play();
        audioSource.volume = volume;
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// ���񭵮�(�i�|�[)
    /// </summary>
    public void PlaySound(string path)
    {
        float Volume = GetVolume();
        //PlayOneShot�A�h�ӭ��ľа_����
        audioSource.PlayOneShot(GetAudio(path), Volume);
        audioSource.volume = Volume;
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
