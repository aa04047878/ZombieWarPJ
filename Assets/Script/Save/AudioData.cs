using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData 
{
    public float bgmVolume; // BGM音量

    public AudioData(float bgmVolume = 0.1f) //初始值設定為0.1f
    {
        this.bgmVolume = bgmVolume;
    }
    
}
