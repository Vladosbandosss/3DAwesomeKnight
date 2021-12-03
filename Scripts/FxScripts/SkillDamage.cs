using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float radius = 0.5f;
    public float damageCount = 10f;

    private EnemyHealth _enemyHealth;
    private bool collided;

    private void Awake()
    {
        
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (var c in hits)
        {
            //if (c.isTrigger)
           // {
            //    continue;
           // }

           _enemyHealth = c.gameObject.GetComponent<EnemyHealth>();
           collided = true;
           

        }

        if (collided)
        {
            _enemyHealth.TakeDamage(damageCount);
        }
    }
}
