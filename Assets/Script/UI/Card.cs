using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
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
    public PlantInfoItem plantInfo;
    /// <summary>
    /// �d���w�b�W���d���̭�
    /// </summary>
    public bool hasUse;
    /// <summary>
    /// �d���w�ϥδN�|�W��
    /// </summary>
    public bool hasLook;
    public bool isMoving;
    /// <summary>
    /// �d���O�_��l��
    /// </summary>
    public bool hasStart;
    private bool gameStart;
    private bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        darkBg = transform.Find("DarkBg").gameObject;
        progressBar = transform.Find("ProgressBar").gameObject;
        card = GetComponent<RectTransform>();

        //�C���٨S�}�l�����Ӱ���
        darkBg.SetActive(false);
        progressBar.SetActive(false);
        //���U�ƥ�
        EventCenter.Instance.AddEventListener(EventType.eventGameStart, GameStart);
        EventCenter.Instance.AddEventListener(EventType.eventGameVictory, GameOver);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameStart)
            return;

        if (!hasStart)
        {
            //��l�ƥd��
            hasStart = true;
            //��ܶi�ױ�
            progressBar.SetActive(true);
            //��ܷt�I��
            darkBg.SetActive(true);
        }

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
        if (gameOver)
            return;

        if (!hasStart)
            return;

        //��ܷt�I���A�N����즲(�شӱ��󤣨�)
        if (darkBg.activeSelf)
        {
            return;
        }
        //�}�l�즲�ɰ���(�ƹ��I�U�����@�����A�I��������)
        Debug.Log("�}�l�즲�ɰ���(�ƹ��I�U�����@����)" + Data.ToString());

        curGameObject = Instantiate(objectprefab);
        //�����I���d�����n��
        SoundManager.instance.PlaySound(Globals.S_Seedlift);
    }

    public void OnDrag(PointerEventData Data)
    {
        if (gameOver)
            return;

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
        if (gameOver)
            return;

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
                    //����شӭ���
                    SoundManager.instance.PlaySound(Globals.S_Plant);
                    //�N�d���]�w���a�O���l����
                    curGameObject.transform.parent = collider.transform;
                    //�شӧ�����}�l�ҰʴӪ�
                    curGameObject.GetComponent<Plant>().SetPlantStart();
                    //���Ӷ���
                    GameManager.instance.ChangeSunNum(-useSun);
                    //���m�p�ɾ�
                    timer = 0;
                    darkBg.SetActive(true);
                    //�M��curGameObject
                    curGameObject = null;
                    break;
                }
                else
                {
                    //�a�O�W�w����L����A����P���d��
                    Debug.Log("�a�O�W�w����L����A�R���d��");
                    Destroy(curGameObject);
                    //curGameObject = null;
                }
            }
            else
            {
                //�S���I����a�O�A����P���d��
                Debug.Log("�S���I��Tag��Land�A�R���d��");
                Destroy(curGameObject);
                //curGameObject = null;
            }

        }

        //���򳣨S�I��A����P���d��
        if (colliders.Length == 0)
        {
            Debug.Log("���򳣨S�I��A�R���d��");
            Destroy(curGameObject);
            //curGameObject = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //�I���d���ɰ���

        //�C���}�l������
        if (gameStart)
            return;

        //���b���ʮɤ����Ʊ�
        if (isMoving)
            return;

        if (hasLook)
            return;

        if (hasUse)
            RemoveCard(gameObject);
        else
            AddCard(gameObject);

    }

    public void RemoveCard(GameObject removeCard)
    {
        ChooseCardPanel chooseCardPanel = UIManager.instance.chooseCardPanel;
       
        if (chooseCardPanel.chooseCardList.Contains(removeCard))
        {
            //���������
            chooseCardPanel.chooseCardList.Remove(removeCard);
            removeCard.GetComponent<Card>().isMoving = true;
            UIManager.instance.chooseCardPanel.UpdateCardPosition();

            //�b���ʥd��
            Transform cardParent = UIManager.instance.allCardPanel.Bg.transform.Find("Card" + removeCard.GetComponent<Card>().plantInfo.plantId.ToString());
            Vector3 curPos = removeCard.transform.position;
            removeCard.transform.SetParent(UIManager.instance.transform, false);
            removeCard.transform.position = curPos; //�����m���O�U�ӡA�]�w��������m�|�]��
            removeCard.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 69);

            //�ϥ�DoMove����
            removeCard.transform.DOMove(new Vector3(cardParent.position.x - 25, cardParent.position.y + 26, cardParent.position.z), 0.3f).OnComplete(() =>
            {
                cardParent.Find("BeforeCard").GetComponent<Card>().darkBg.SetActive(false);
                cardParent.Find("BeforeCard").GetComponent<Card>().hasLook = false;
                //���ʧ���R������
                removeCard.GetComponent<Card>().isMoving = false;
                Destroy(removeCard);
            });
        }

        
    }

    public void AddCard(GameObject gameObject)
    {
        ChooseCardPanel chooseCardPanel = UIManager.instance.chooseCardPanel;
        Debug.Log($"��ܦ�m : {chooseCardPanel.transform.position}");
        int curIndex = chooseCardPanel.chooseCardList.Count;
        Debug.Log($"��e��d���ƶq : {curIndex}");
        //�p�G��d���w���A�h������
        if (curIndex >= 8)
        {
            Debug.Log("��d���w���A�L�k�K�[�d��");
            return;
        }

        hasLook = true;
        darkBg.SetActive(true);
        //�p�G��d��줣���A�h�K�[�d��
        GameObject useCard = Instantiate(plantInfo.cardPrefab);
        useCard.transform.SetParent(UIManager.instance.transform);
        useCard.transform.position = transform.position;
        useCard.GetComponent<RectTransform>().sizeDelta = new Vector2(41, 56);
        useCard.name = "Card";
        // ��T����
        useCard.GetComponent<Card>().plantInfo = plantInfo;
        //���ʨ�ؼЦ�m
        Transform targetObject = chooseCardPanel.cards.transform.Find("BeforeCard" + curIndex);
        Debug.Log("�ؼЦ�m�G" + targetObject.name);
        useCard.GetComponent<Card>().isMoving = true;
        useCard.GetComponent<Card>().hasUse = true;
        chooseCardPanel.chooseCardList.Add(useCard);

        //�ϥ�DoTwing��DoMove��k���ʨ�ؼЦ�m
        useCard.transform.DOMove(new Vector3(targetObject.position.x - 25, targetObject.position.y + 26, targetObject.position.z), 0.3f).OnComplete(() =>
        {
            //���ʧ�����A�N�d����������]�m����d���
            useCard.transform.SetParent(targetObject);
            //useCard.transform.localPosition = Vector2.zero;
            useCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            useCard.GetComponent<Card>().isMoving = false;
        });
    }

    private void GameStart()
    {
        gameStart = true;
    }

    private void GameOver()
    {
        gameOver = true;
    }
}
