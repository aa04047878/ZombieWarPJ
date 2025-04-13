using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float damage;
    public Player player;
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Zombie"))
        {
            hit.transform.GetComponent<ZombieNormal>().ChangeHealth(-damage);
            player.ChangeHealth(-10);
        }
    }
}
