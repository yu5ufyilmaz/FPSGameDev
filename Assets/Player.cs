using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public int HP = 100;
   
   public void TakeDamage(int damageAmount)
   {
      HP -= damageAmount;

      if (HP <= 0)
      {
         Debug.Log("Dead");
      }
      else
      {
         Debug.Log("Hit");
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("AlienHand"))
      {
         TakeDamage(other.gameObject.GetComponent<AlienHand>().damage);
      }
   }
}
