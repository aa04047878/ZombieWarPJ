using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int id;
    public bool isSelected = false;
    public Sprite character;
    private StagePanel parent;
    private Image selectFrame;

    private void Awake()
    {
        selectFrame = UITool.GetUIComponent<Image>(this.gameObject, "SelectFrame");
        selectFrame.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        parent = UITool.GetParentComponent<StagePanel>("StagePanel");
        if (parent != null)
            Debug.Log("��parent");
        else
            Debug.Log("�S��parent");
        parent.AddStageItemDic(id, this);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //isSelected = !isSelected;
        parent.curItemId = id; 
        parent.stageCharacterPreview.sprite = character;
        
        // Handle selection logic here
        //Debug.Log($"AvatarItem {id} selected.");
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        //isSelected = false;
        // Handle deselection logic here
        //Debug.Log($"AvatarItem {id} deselected.");
    }

    public void RefreshSelect()
    {
        selectFrame.gameObject.SetActive(id == parent.curItemId);
    }
}
