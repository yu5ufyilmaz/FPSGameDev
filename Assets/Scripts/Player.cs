using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
   public int HP = 100;
   public GameObject bloodyScreen;

   public TextMeshProUGUI playerHealthUI;
   public GameObject gameOverUI;

   public bool isDead;

   private void Start()
   {
     // playerHealthUI.text = $"Health: {HP}";
   }

   public void TakeDamage(int damageAmount)
   {
      HP -= damageAmount;

      if (HP <= 0)
      {
         Debug.Log("Dead");
         PlayerDead();
         isDead = true;
      }
      else
      {
         Debug.Log("Hit");
         StartCoroutine(BloodyScreenEffect());
         playerHealthUI.text = $"Health: {HP}";
         SoundManager.instance.playerChannel.PlayOneShot(SoundManager.instance.playerHurt);
      }
   }

   private void PlayerDead()
   {
      SoundManager.instance.playerChannel.PlayOneShot(SoundManager.instance.playerDeath);
      
      SoundManager.instance.playerChannel.clip = SoundManager.instance.gameOverMusic;
      SoundManager.instance.playerChannel.PlayDelayed(2f);


      
      GetComponent<PlayerMovement>().enabled = false;
      GetComponent<MouseMovement>().enabled = false;

      GetComponentInChildren<Animator>().enabled = true;
      playerHealthUI.gameObject.SetActive(false);
      
      GetComponent<ScreenFader>().StartFade();

      StartCoroutine(ShowGameOverUI());
   }

   private IEnumerator ShowGameOverUI()
   {
      yield return new WaitForSeconds(1f);
      gameOverUI.gameObject.SetActive(true);
      int waveSurvived = GlobalReferences.instance.waveNumber;
      
      SaveLoadManager.instance.SaveHighScore(waveSurvived-1);

      StartCoroutine(ReturnToMainMenu());
   }

   private IEnumerator ReturnToMainMenu()
   {
      yield return new WaitForSeconds(8f);

      SceneManager.LoadScene("MainMenu");
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
         if (isDead == false)
         {
            TakeDamage(other.gameObject.GetComponent<AlienHand>().damage);
         }
      }
   }
}
