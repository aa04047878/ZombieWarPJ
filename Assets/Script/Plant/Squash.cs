using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    None,
    Down,
    Up,
    Over,
}

public class Squash : Plant
{
    public float findZombieDistance;
    private int line;
    private Vector3 attackPos;
    private State squashState;
    public float damage;
    protected override void Start()
    {
        base.Start();
        squashState = State.None;        
    }

    protected override void Update()
    {
        base.Update();
        
        switch (squashState)
        {
            case State.Down:
                break;
            case State.Up:
                //���ʦܧ�����m�W��100���
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(attackPos.x, attackPos.y + 100), Time.deltaTime * 200);
                break;
            case State.Over:
                //�q������m�W�詹������m����
                transform.position  = Vector2.MoveTowards(transform.position, attackPos, Time.deltaTime * 200);
                break;
            default:
                break;
        }
    }

    public void CheckZombieInRange()
    {
        // �w�g�������ؼСA�h���������ާ@
        if (attackPos != Vector3.zero)
        {
            return;
        }

        // ���o��e�Ӫ�����Ʀ��h���L��
        List<GameObject> zombies =  GameManager.instance.GetLineZombies(line);

        //�S���L�͡A�ƻ�Ƴ����ΰ�
        if (zombies.Count == 0)
        {
            return;
        }

        float minDis = findZombieDistance;
        GameObject nearZombie = null;

        // ���o�̪��L��
        for (int i = 0; i < zombies.Count; i++)
        {
            // ���o�L�ͪ��Z��
            float dis = Vector2.Distance(zombies[i].transform.position, transform.position);
            if (dis <= minDis)
            {
                minDis = dis;
                nearZombie = zombies[i];
            }
        }

        // �p�G�S���L�ͦb�d�򤺡A�h���������ާ@
        if (nearZombie == null)
            return;

        // �p�������m
        attackPos = nearZombie.transform.position;
        // �Ǫ����V���Ӥ�V
        DoSquashLook();
        //�����}�l
        SetAttackDown();
    }

    public void DoSquashLook()
    {
        string lookDir = "Right";
        if (attackPos.x < transform.position.x)
        {
            lookDir = "Left";
        }
        ani.SetTrigger(lookDir);
    }

    public override void SetPlantStart()
    {
        base.SetPlantStart();
        //�شӧ�����~�ݭn�����Ʊ�
        boxCollider2D.enabled = false;
        line = GameManager.instance.GetPlantLine(this.gameObject);
        InvokeRepeating("CheckZombieInRange", 1, 0.5f);
    }

    public override float ChangeHealth(float num)
    {
        //����n�ʳy���ˮ`
        return health;
    }

    private void SetAttackDown()
    {
        squashState = State.Down;
        ani.SetTrigger("Attack_Down");
    }

    /// <summary>
    /// Up����(�ʵe�ƥ�)
    /// </summary>
    private void SetAttatkUp()
    {
        squashState = State.Up;
        ani.SetTrigger("Attack_Up");
    }

    /// <summary>
    /// Over����(�ʵe�ƥ�)
    /// </summary>
    private void SetAttatkOver()
    {
        squashState = State.Over;
        ani.SetTrigger("Attack_Over");
    }

    /// <summary>
    /// �ǳƧ���(�ʵe�ƥ�)
    /// </summary>
    private void DoReallyAttack()
    {
        boxCollider2D.enabled = true;
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Zombie"))
        {
            hit.GetComponent<ZombieNormal>().ChangeHealth(-damage);
        }
    }
}
