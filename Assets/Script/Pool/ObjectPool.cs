using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    /*
    自己寫的物件池和unity內建的物件池的差別
    我有預熱，unity內建的沒有
    但是沒事不用去增加子彈的初始化工作量，沒事別搞死自己!!!

    物件池的目的 : 避免在玩遊戲的時候進行Instantiate 和 Destroy ，若預置物的內容很大，專案又很大，就容易造成卡頓的情況。
                   所以物件池要做的事情就是在初始化的時候先預熱，要使用的時候再拿出來用，避免再玩遊戲的時候卡頓。
    */

    /// <summary>
    /// 物件池庫存
    /// </summary>
    private Queue<T> objectQueue;
    private GameObject _prefab;
    private static ObjectPool<T> _instance;
    public static ObjectPool<T> Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ObjectPool<T>();
            return _instance;
        }
    }

    public int QueueCount { get { return objectQueue.Count; } }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="prefab">預置物</param>
    /// <param name="warmUpCount">預熱時間(物件池優化)</param>
    public void InitPool(GameObject prefab, int warmUpCount = 0)
    {
        _prefab = prefab;
        objectQueue = new Queue<T>();

        //物件池預熱(先製造好東西以後，放入物件池內)
        List<T> warmUplist = new List<T>();
        for (int i = 0; i < warmUpCount; i++)
        {
            T t = Instance.Spawn(Vector3.zero, Quaternion.identity);
            warmUplist.Add(t);
        }

        for (int i = 0; i < warmUplist.Count; i++)
        {
            Instance.Recycle(warmUplist[i]);
        }

        
    }

    /// <summary>
    /// 製造物件
    /// </summary>
    /// <param name="position"></param>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public T Spawn(Vector3 position, Quaternion quaternion)
    {
        if (_prefab == null)
        {
            Debug.Log(typeof(T).ToString() + "prefab not set");
            return default(T);
        }

        //檢查物件池庫存有無東西，沒東西就製造出來
        if (QueueCount <= 0)
        {
            GameObject g = Object.Instantiate(_prefab, position, quaternion);
            T t = g.GetComponent<T>();
            if (t == null)
            {
                Debug.Log(typeof(T).ToString() + "not found in prefab");
                return default(T);
            }
            objectQueue.Enqueue(t);
        }

        //把物件池庫存裡面的東西拿出來用
        T obj = objectQueue.Dequeue();
        obj.gameObject.transform.position = position;
        obj.gameObject.transform.rotation = quaternion;
        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 回收物件
    /// </summary>
    /// <param name="obj"></param>
    public void Recycle(T obj)
    {
        objectQueue.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }

    /// <summary>
    /// 取得物件池庫存
    /// </summary>
    /// <returns></returns>
    public Queue<T> GetInventory()
    {
        return objectQueue;
    }
}
