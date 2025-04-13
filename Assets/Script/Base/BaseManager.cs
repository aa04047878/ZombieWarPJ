using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    public string curUserName;

    //單利模式
    private static BaseManager instance;
    public static BaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BaseManager();
            }
            return instance;
        }
    }

    public BaseManager()
    {
        curUserName = GetClientData().curUserName;
    }

    public ClientData GetClientData()
    {
        return LocalConfig.LoadClientData();
    }

    public void SetCurUserName(string name)
    {
        curUserName = name;

        //儲存
        ClientData clientData = GetClientData();
        clientData.curUserName = name;
        LocalConfig.SaveClientData(clientData);

        //通知事件已發生
        EventCenter.Instance.EventTrigger(EventType.eventCurUserChange, curUserName);
    }
}
