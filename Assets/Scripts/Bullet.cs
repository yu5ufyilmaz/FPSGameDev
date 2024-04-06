using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    
    
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
        
        if (collision.gameObject.CompareTag("Alien"))
        {
            if (collision.gameObject.GetComponent<Enemies>().isDead == false)
            {
                collision.gameObject.GetComponent<Enemies>().TakeDamage(bulletDamage);
            }
            
            CreateBloodSprayEffect(collision);
            
            Destroy(gameObject);
        }
    }

    private void CreateBloodSprayEffect(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(GlobalReferences.instance.bloodSprayEffect,contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
        
        bloodSprayPrefab.transform.SetParent(collision.gameObject.transform);
    }

    void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];

        GameObject hole = Instantiate(GlobalReferences.instance.bulletImpactEffectPrefab,contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
        
        hole.transform.SetParent(collision.gameObject.transform);
    }
    
}
