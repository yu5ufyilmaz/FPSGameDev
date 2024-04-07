using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemies : MonoBehaviour
{
   [SerializeField] private int HP = 100;
   private Animator animator;
    
    private NavMeshAgent navAgent;

    public bool isDead;

   private void Start()
   {
      animator = GetComponent<Animator>();
      navAgent = GetComponent<NavMeshAgent>();
   }

   public void TakeDamage(int damageAmount)
   {
      HP -= damageAmount;

      if (HP <= 0)
      {
         int randomValue = Random.Range(0, 2);

         if (randomValue == 0)
         {
            animator.SetTrigger("Die1");
         }
         else
         {
            animator.SetTrigger("Die2");
         }
         
         isDead = true;
         SoundManager.instance.alienChannel2.PlayOneShot(SoundManager.instance.alienDeath);
      }
      else
      {
         animator.SetTrigger("Damage");
         SoundManager.instance.alienChannel2.PlayOneShot(SoundManager.instance.alienHurt);
      }

      

   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position,2.5f);
      
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position,100f);
      
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(transform.position,101f);


   }
}
