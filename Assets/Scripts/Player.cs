using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
   public int HP = 100;
   public GameObject bloodyScreen;
   
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
         StartCoroutine(BloodyScreenEffect());
      }
   }

   private IEnumerator BloodyScreenEffect()
   {
      if (bloodyScreen.activeInHierarchy == false)
      {
         bloodyScreen.SetActive(true);
      }

      var image = bloodyScreen.GetComponentInChildren<Image>();

      Color startColor = image.color;
      startColor.a = 1f;
      image.color = startColor;

      float duration = 3f;
      float elapsedTime = 0f;

      while (elapsedTime < duration)
      {
         float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

         Color newColor = image.color;
         newColor.a = alpha;
         image.color = newColor;

         elapsedTime += Time.deltaTime;

         yield return null;
      }

      if (bloodyScreen.activeInHierarchy)
      {
         bloodyScreen.SetActive(false);
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
