using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
{
    public GameObject groundImpactSpawn,kickFXSpawn,fireTornadoSpawn,fireShieldSpawn;

    public GameObject groundInmpactPrefab,
        kickFxPrefab,
        fireTornadoPrefab,
        fireShieldPrefab,
        healFxPrefab,
        thunderFXprefab;//
  

    void GroundImpact()
    {
        Instantiate(groundInmpactPrefab, groundImpactSpawn.transform.position, Quaternion.identity);
    }

    void Kick()
    {
        Instantiate(kickFxPrefab, kickFXSpawn.transform.position, Quaternion.identity);
    }

    void FireTornado()
    {
        Instantiate(fireTornadoPrefab, fireTornadoSpawn.transform.position, Quaternion.identity);
    }

    void FireShield()
    {
        GameObject fireObg = Instantiate(fireShieldPrefab, fireShieldSpawn.transform.position, Quaternion.identity);
        fireObg.transform.SetParent(transform);
    }

    void Heal()
    {
        Vector3 temp = transform.position;
        temp.y += 2f;
        GameObject healObg = Instantiate(healFxPrefab, temp, Quaternion.identity);
        healObg.transform.SetParent(transform);
    }

    void ThunderAttack()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 pos=Vector3.zero;
            if (i == 0)
            {
                pos = new Vector3(transform.position.x - 4f, transform.position.y + 2f, transform.position.z);
            }else if (i == 1)
            {
                pos = new Vector3(transform.position.x +4f, transform.position.y + 2f, transform.position.z);
            }else if (i == 2)
            {
                pos = new Vector3(transform.position.x , transform.position.y + 2f, transform.position.z-4f);
            }else if (i == 3)
            {
                pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z+4f);
            }else if (i == 4)
            {
                pos = new Vector3(transform.position.x +2.5f, transform.position.y + 2f, transform.position.z+2.5f);
            }
            else if (i == 5)
            {
                pos = new Vector3(transform.position.x -2.5f, transform.position.y + 2f, transform.position.z+2.5f);
            }
            else if (i == 6)
            {
                pos = new Vector3(transform.position.x -2.5f, transform.position.y + 2f, transform.position.z-2.5f);
            }
            else if (i == 7)
            {
                pos = new Vector3(transform.position.x +2.5f, transform.position.y + 2f, transform.position.z+2.5f);
            }
            Instantiate(thunderFXprefab, pos, Quaternion.identity);
            
        }
    }
}
