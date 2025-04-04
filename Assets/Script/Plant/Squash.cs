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
                //移動至攻擊位置上方100單位
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(attackPos.x, attackPos.y + 100), Time.deltaTime * 200);
                break;
            case State.Over:
                //從攻擊位置上方往攻擊位置移動
                transform.position  = Vector2.MoveTowards(transform.position, attackPos, Time.deltaTime * 200);
                break;
            default:
                break;
        }
    }

    public void CheckZombieInRange()
    {
        // 已經有攻擊目標，則不執行任何操作
        if (attackPos != Vector3.zero)
        {
            return;
        }

        // 取得當前植物的行數有多少殭屍
        List<GameObject> zombies =  GameManager.instance.GetLineZombies(line);

        //沒有殭屍，甚麼事都不用做
        if (zombies.Count == 0)
        {
            return;
        }

        float minDis = findZombieDistance;
        GameObject nearZombie = null;

        // 取得最近的殭屍
        for (int i = 0; i < zombies.Count; i++)
        {
            // 取得殭屍的距離
            float dis = Vector2.Distance(zombies[i].transform.position, transform.position);
            if (dis <= minDis)
            {
                minDis = dis;
                nearZombie = zombies[i];
            }
        }

        // 如果沒有殭屍在範圍內，則不執行任何操作
        if (nearZombie == null)
            return;

        // 計算攻擊位置
        attackPos = nearZombie.transform.position;
        // 怪物面向哪個方向
        DoSquashLook();
        //攻擊開始
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
        //種植完成後才需要做的事情
        boxCollider2D.enabled = false;
        line = GameManager.instance.GetPlantLine(this.gameObject);
        InvokeRepeating("CheckZombieInRange", 1, 0.5f);
    }

    public override float ChangeHealth(float num)
    {
        //不對南瓜造成傷害
        return health;
    }

    private void SetAttackDown()
    {
        squashState = State.Down;
        ani.SetTrigger("Attack_Down");
    }

    /// <summary>
    /// Up攻擊(動畫事件)
    /// </summary>
    private void SetAttatkUp()
    {
        squashState = State.Up;
        ani.SetTrigger("Attack_Up");
    }

    /// <summary>
    /// Over攻擊(動畫事件)
    /// </summary>
    private void SetAttatkOver()
    {
        squashState = State.Over;
        ani.SetTrigger("Attack_Over");
    }

    /// <summary>
    /// 準備攻擊(動畫事件)
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
