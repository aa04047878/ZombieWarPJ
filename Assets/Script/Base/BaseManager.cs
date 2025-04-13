using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    public string curUserName;

    //��Q�Ҧ�
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

        //�x�s
        ClientData clientData = GetClientData();
        clientData.curUserName = name;
        LocalConfig.SaveClientData(clientData);

        //�q���ƥ�w�o��
        EventCenter.Instance.EventTrigger(EventType.eventCurUserChange, curUserName);
    }
}
