using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{
    // 事件中心的接口
}

// 无参数事件响应
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

// 带参数事件响应
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventCenter 
{
    /*
    EventCenter(事件中心) : 
        1. 事件中心作為所有事件的管理器，在遊戲中是唯一的，必須設為單利模式。
        2. 事件中心最重要的2個功能，"訂閱" 和 "通知"。
    */

    private static EventCenter instance;

    public static EventCenter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventCenter();
            }
            return instance;
        }
    }

    /// <summary>
    /// 事件字典(儲存所有已訂閱的事件)
    /// </summary>
    private Dictionary<string, IEventInfo> _eventDic = new Dictionary<string, IEventInfo>();

    #region 訂閱
    /// <summary>
    /// 訂閱(無參數)
    /// </summary>
    /// <param name="name">訂閱的名稱</param>
    /// <param name="action">訂閱的列表</param>
    public void AddEventListener(string name, UnityAction action)
    {
        if (_eventDic.ContainsKey(name))
            (_eventDic[name] as EventInfo).actions += action;
        else
            _eventDic.Add(name, new EventInfo(action));
    }

    /// <summary>
    /// 取消訂閱(無參數)
    /// </summary>
    /// <param name="name">訂閱的名稱</param>
    /// <param name="action">訂閱的列表</param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (_eventDic.ContainsKey(name))
            (_eventDic[name] as EventInfo).actions -= action;
    }

    /// <summary>
    /// 訂閱(有參數)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">訂閱的名稱(訂閱主題)</param>
    /// <param name="action">訂閱的列表(事件發生時要做的事情)</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        // 舊事件
        if (_eventDic.ContainsKey(name))
            (_eventDic[name] as EventInfo<T>).actions += action;
        // 新事件
        else
            _eventDic.Add(name, new EventInfo<T>(action));
    }

    /// <summary>
    /// 取消訂閱(有參數)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">訂閱的名稱</param>
    /// <param name="action">訂閱的列表</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (_eventDic.ContainsKey(name))
            (_eventDic[name] as EventInfo<T>).actions -= action;
    }
    #endregion

    #region 通知
    /// <summary>
    /// 通知事件已發生(無參數)
    /// </summary>
    /// <param name="name">事件名子</param>
    public void EventTrigger(string name)
    {
        if (_eventDic.ContainsKey(name))
            if ((_eventDic[name] as EventInfo).actions != null)
                (_eventDic[name] as EventInfo).actions.Invoke();
    }

    /// <summary>
    /// 通知事件已發生(有參數)
    /// </summary>
    /// <typeparam name="T">參數</typeparam>
    /// <param name="name">事件名子</param>
    /// <param name="info">參數</param>
    public void EventTrigger<T>(string name, T info)
    {
        if (_eventDic.ContainsKey(name))
            if ((_eventDic[name] as EventInfo<T>).actions != null)
                (_eventDic[name] as EventInfo<T>).actions.Invoke(info);
    }
    #endregion

    // 清空事件監聽
    // 主要用於場景切切換時防止内存泄漏
    public void Clear()
    {
        _eventDic.Clear();
    }
}

public class EventType
{
    /// <summary>
    /// 創建使用者事件
    /// </summary>
    public const string eventNewUserCreate = "EventNewUserCreate"; 
    /// <summary>
    /// 當前使用者改變事件
    /// </summary>
    public const string eventCurUserChange = "EventCurUserChange";
    /// <summary>
    /// 使用者刪除事件
    /// </summary>
    public const string eventUserDelete = "EventUserDelete";
    /// <summary>
    /// 玩家死亡事件
    /// </summary>
    public const string eventPlayerDie = "EventPlayerDie";
    /// <summary>
    /// 遊戲開始事件
    /// </summary>
    public const string eventGameStart = "EventGameStart";


}

