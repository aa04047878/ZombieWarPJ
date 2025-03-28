using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform card;
    /// <summary>
    /// �d�����w�m��
    /// </summary>
    public GameObject objectprefab;
    /// <summary>
    /// �ͦ��X�Ӫ��d������
    /// </summary>
    public GameObject curGameObject;
    /// <summary>
    /// �ܷt�I��(�N�o�����ϥ�)
    /// </summary>
    private GameObject darkBg;
    /// <summary>
    /// �i�ױ�
    /// </summary>
    private GameObject progressBar;
    /// <summary>
    /// ���d���ݭn�������ƶq
    /// </summary>
    public int useSun;
    public float waitTime;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        darkBg = transform.Find("DarkBg").gameObject;
        progressBar = transform.Find("ProgressBar").gameObject;
        card = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        UpdateProgress();
        UpdateDarkBg();
    }

    /// <summary>
    /// ��s�i�ױ�
    /// </summary>
    private void UpdateProgress()
    {
        float per = Mathf.Clamp(timer / waitTime, 0, 1);

        //�i�ױ��Ѿl�ɶ����
        progressBar.GetComponent<Image>().fillAmount = 1 - per;
    }

    /// <summary>
    /// ��s�t�I��
    /// </summary>
    private void UpdateDarkBg()
    {
        //�ˬd�O�_�N�o���� && �����ƶq����
        if (progressBar.GetComponent<Image>().fillAmount == 0 && GameManager.instance.sunNum >= useSun)
        {
            //�N�o�����A���÷t�I��
            darkBg.SetActive(false);
        }
        else
        {
            //�N�o���A��ܷt�I��
            darkBg.SetActive(true);
        }
    }

    public void OnBeginDrag(PointerEventData Data)
    {
        //��ܷt�I���A�N����즲(�شӱ��󤣨�)
        if (darkBg.activeSelf)
        {
            return;
        }
        //�}�l�즲�ɰ���(�ƹ��I�U�����@�����A�I��������)
        Debug.Log("�}�l�즲�ɰ���(�ƹ��I�U�����@����)" + Data.ToString());

        curGameObject = Instantiate(objectprefab);
    }

    public void OnDrag(PointerEventData Data)
    {   
        //��ܷt�I���A�N����즲(�شӱ��󤣨�)
        if (darkBg.activeSelf)
        {
            return;
        }

        //���b�즲�ɰ���(�ƹ�������)
        Debug.Log("���b�즲�ɰ���(�ƹ�������)");
        if (curGameObject == null)
            return;
        
        // ����ƹ��b�ù��W����m
        Vector3 screenPosition = Input.mousePosition;

        // �p�G�O 3D �����A�������ѥ��T�� Z �`��
        screenPosition.z = 10f; // �]�m�`�ס]�Z����v�����Z���^

        // �N�ù��y���ഫ���@�ɮy��
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //�즲�Ӫ�
        curGameObject.transform.position = worldPosition;
        
    }

    public void OnEndDrag(PointerEventData Data)
    {
        //��ܷt�I���A�N����즲(�شӱ��󤣨�)
        if (darkBg.activeSelf)
        {
            return;
        }
        //�����즲�ɰ���(�ƹ���}�ɰ���)
        Debug.Log("�����즲�ɰ���(�ƹ���}�ɰ���)");
        //�ˬd�ƹ���m���@�ɮy��(curGameObject����m)�O�_���I����
        Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        foreach (var collider in colliders)
        {
            
            //�ݼиI�쪺�I���馳�a�O
            if (collider.tag == "Land")
            {
                //�B�a�O�W�S����L����
                if (collider.transform.childCount == 0)
                {
                    Debug.Log("�I���쪺����W�١G" + collider.name);
                    //�����m�d��
                    curGameObject.transform.position = collider.transform.position;
                    //�N�d���]�w���a�O���l����
                    curGameObject.transform.parent = collider.transform;
                    //���Ӷ���
                    GameManager.instance.ChangeSunNum(-useSun);
                    //�M��curGameObject
                    curGameObject = null;
                    break;
                }
                else
                {
                    //�S���I����a�O�A����P���d��
                    Destroy(curGameObject);
                    curGameObject = null;
                }
            }

        }
    }

    
}
