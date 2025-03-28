using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text sunNumText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitUI()
    {
        sunNumText.text = GameManager.instance.sunNum.ToString();
    }

    public void UpdateUI()
    {
        sunNumText.text = GameManager.instance.sunNum.ToString();
    }
}
