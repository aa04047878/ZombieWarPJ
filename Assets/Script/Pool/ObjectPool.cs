using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    /*
    �ۤv�g��������Munity���ت���������t�O
    �ڦ��w���Aunity���ت��S��
    ���O�S�Ƥ��Υh�W�[�l�u����l�Ƥu�@�q�A�S�ƧO�d���ۤv!!!

    ��������ت� : �קK�b���C�����ɭԶi��Instantiate �M Destroy �A�Y�w�m�������e�ܤj�A�M�פS�ܤj�A�N�e���y���d�y�����p�C
                   �ҥH������n�����Ʊ��N�O�b��l�ƪ��ɭԥ��w���A�n�ϥΪ��ɭԦA���X�ӥΡA�קK�A���C�����ɭԥd�y�C
    */

    /// <summary>
    /// ������w�s
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
    /// ��l��
    /// </summary>
    /// <param name="prefab">�w�m��</param>
    /// <param name="warmUpCount">�w���ɶ�(������u��)</param>
    public void InitPool(GameObject prefab, int warmUpCount = 0)
    {
        _prefab = prefab;
        objectQueue = new Queue<T>();

        //������w��(���s�y�n�F��H��A��J�������)
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
    /// �s�y����
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

        //�ˬd������w�s���L�F��A�S�F��N�s�y�X��
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

        //�⪫����w�s�̭����F�讳�X�ӥ�
        T obj = objectQueue.Dequeue();
        obj.gameObject.transform.position = position;
        obj.gameObject.transform.rotation = quaternion;
        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// �^������
    /// </summary>
    /// <param name="obj"></param>
    public void Recycle(T obj)
    {
        objectQueue.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���o������w�s
    /// </summary>
    /// <returns></returns>
    public Queue<T> GetInventory()
    {
        return objectQueue;
    }
}
