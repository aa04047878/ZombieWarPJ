using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanel : MonoBehaviour
{
    private GameObject progress;
    private GameObject head;
    private GameObject levelTest;
    private GameObject Bg;
    private GameObject flag;
    private GameObject flagPrefab;
    // Start is called before the first frame update
    void Start()
    {
        progress = transform.Find("Progress").gameObject;
        head = transform.Find("Head").gameObject;
        levelTest = transform.Find("LevelTest").gameObject;
        Bg = transform.Find("Bg").gameObject;
        flag = transform.Find("Flag").gameObject;
        flagPrefab = Resources.Load<GameObject>("Prefab/Flag");
        //SetPrecent(0.5f);
        //SetFlagPercent(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetPrecent(float per)
    {
        //�i�ױ�
        progress.GetComponent<Image>().fillAmount = per;
        //�i�ױ��̥k�䪺��m = �I�����@�ɮy�Ц�m + �I�����e�ת��@�b
        float originPosX = Bg.GetComponent<RectTransform>().position.x + Bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        Debug.Log($"Bg.position.x : {Bg.GetComponent<RectTransform>().position.x} , Bg.sizeDelta.x / 2 : {Bg.GetComponent<RectTransform>().sizeDelta.x / 2}");
        Debug.Log($"originPosX : {originPosX}");
        //�i�ױ��e�� = �I�����e�� 
        float width = Bg.GetComponent<RectTransform>().sizeDelta.x;
        //�i�ױ��Y���������q
        float offect = 0;
        //�]�m�Y����m = �̥k�䪺��m + �����q
        //head.GetComponent<RectTransform>().position = new Vector2(originPosX + offect, head.GetComponent<RectTransform>().position.y);
        //�]�m�Y����m = �̥k�䪺��m - �i�צʤ��񴫺�ӱo������ + �����q
        head.GetComponent<RectTransform>().position = new Vector2(originPosX - per * width + offect, head.GetComponent<RectTransform>().position.y);

    }

    public void SetFlagPercent(float per)
    {
        flag.SetActive(false);
        //�i�ױ��̥k�䪺��m = �I�����@�ɮy�Ц�m + �I�����e�ת��@�b
        float originPosX = Bg.GetComponent<RectTransform>().position.x + Bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        //�i�ױ��e�� = �I�����e�� 
        float width = Bg.GetComponent<RectTransform>().sizeDelta.x;
        //�i�ױ��X�l�������q
        float offect = 0;
        //�гy�s�X�l
        GameObject newFlag = Instantiate(flagPrefab);
        //�]�m�X�l��������
        newFlag.transform.SetParent(gameObject.transform, false);
        //�]�m�X�l����m
        newFlag.GetComponent<RectTransform>().position = flag.GetComponent<RectTransform>().position;
        //�]�m�X�l��m = �̥k�䪺��m - �i�ױ��e�ת��@�b + �����q
        newFlag.GetComponent<RectTransform>().position = new Vector2(originPosX - per * width + offect, newFlag.GetComponent<RectTransform>().position.y);
    }
}
