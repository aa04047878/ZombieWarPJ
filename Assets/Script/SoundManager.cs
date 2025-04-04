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
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dictAudio = new Dictionary<string, AudioClip>();
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
    /// ���񭵮�(�i�|�[)
    /// </summary>
    public void PlaySound(string path, float value = 0.1f)
    {
        //PlayOneShot�A�h�ӭ��ľа_����
        audioSource.PlayOneShot(GetAudio(path), value);
        audioSource.volume = value;
    }
}
