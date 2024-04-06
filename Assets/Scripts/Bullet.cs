using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(collision.gameObject.name);
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("hit a wall");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];

        GameObject hole = Instantiate(GlobalReferences.instance.bulletImpactEffectPrefab,contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
        
        hole.transform.SetParent(collision.gameObject.transform);
    }
    
}
