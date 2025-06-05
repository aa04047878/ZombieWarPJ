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
            ReflashSelectState(); // ��s��ܪ��A
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
        //���o�R�x���
        StageData stageData = LocalConfig.LoadStageData();
        //�]�w�R�x����Ϥ�
        spriteData = GetStageCharacterData(stageData.id); // Example to get the spriteData for Peashooter
        stageCharacterPreview.sprite = spriteData;
        curItemId = stageData.id; // �q�x�s�ɮ׸̨��oID�ӳ]�w
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
    /// ��s��ܪ��A
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
        //�x�s�R�x���
        StageData stageData = LocalConfig.LoadStageData();
        stageData.id = id;
        LocalConfig.SaveStageData(stageData);
        //�ܧ�MainMenuPanel���R�x����
        EventCenter.Instance.EventTrigger(EventType.eventChangeStageCharacter, id);

    }

    private void OnBtnBack()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.stagePanel);
    }
}
