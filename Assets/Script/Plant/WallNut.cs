using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : Plant
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override float ChangeHealth(float num)
    {
        float currentHealth = base.ChangeHealth(num);
        //�]�m�ʵe�Ѽ�
        ani.SetFloat("FloatPercent", currentHealth / health);
        return currentHealth;
    }
}
