using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : BasePanel
{
    private Dictionary<int, string> stageItemSpriteDic;
    private Dictionary<int, AvatarItem> stageItemDic;
    public Sprite spriteData;    
    public Image stageCharacterPreview;
    private Button btnOk;
    private int _curItemId;
    private Button btnBack;
    public int curItemId
    {
        get { return _curItemId; }
        set
        {
            _curItemId = value;
            ReflashSelectState(); // 刷新選擇狀態
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        transform.name = "StagePanel";
        stageItemSpriteDic = new Dictionary<int, string>();
        stageItemSpriteDic.Add(0, Globals.PeashooterAllbody);
        stageItemSpriteDic.Add(1, Globals.SunflowerAllbody);
        stageItemSpriteDic.Add(2, Globals.WallNutAllbody);
        stageItemSpriteDic.Add(4, Globals.SquashAllbody);
        stageItemSpriteDic.Add(5, Globals.PurpleMushroomAllbody);
        stageItemDic = new Dictionary<int, AvatarItem>();
        btnOk = UITool.GetUIComponent<Button>(this.gameObject, "BtnOk");
        btnBack = UITool.GetUIComponent<Button>(this.gameObject, "BtnBack");
    }

    protected override void Start()
    {
        base.Start();
        btnOk.onClick.AddListener(() => OnBtnOk(curItemId));
        btnBack.onClick.AddListener(() => OnBtnBack());
        //取得舞台資料
        StageData stageData = LocalConfig.LoadStageData();
        //設定舞台角色圖片
        spriteData = GetStageCharacterData(stageData.id); // Example to get the spriteData for Peashooter
        stageCharacterPreview.sprite = spriteData;
        curItemId = stageData.id; // 從儲存檔案裡取得ID來設定
    }

    protected override void Update()
    {
        base.Update();
    }

    public Sprite GetStageCharacterData(int id)
    {
        if (stageItemSpriteDic.ContainsKey(id))
        {
            return Resources.Load<Sprite>(stageItemSpriteDic[id]);
        }
        else
        {
            Debug.Log("Stage character not found for id: " + id);
            return null;
        }
    }

    public void AddStageItemDic(int id, AvatarItem avatarItem)
    {
        //stageItemDic[id] = avatarItem;
        stageItemDic.Add(id, avatarItem);
    }

    /// <summary>
    /// 刷新選擇狀態
    /// </summary>
    private void ReflashSelectState()
    {
        foreach (var item in stageItemDic)
        {
            item.Value.RefreshSelect();
        }
    }

    private void OnBtnOk(int id)
    {
        //儲存舞台資料
        StageData stageData = LocalConfig.LoadStageData();
        stageData.id = id;
        LocalConfig.SaveStageData(stageData);
        //變更MainMenuPanel的舞台角色
        EventCenter.Instance.EventTrigger(EventType.eventChangeStageCharacter, id);

    }

    private void OnBtnBack()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.stagePanel);
    }
}
