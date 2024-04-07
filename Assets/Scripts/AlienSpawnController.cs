using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienSpawnController : MonoBehaviour
{
    public int initialAliensPerWave = 5;
    public int currentAliensPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldown = 10f;

    public bool inCooldown;
    public float coolDownCounter = 0;

    public List<Enemies> currentAliensAlive;

    public GameObject alienPrefab;

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI coolDownCounterUI;

    public TextMeshProUGUI currentWaveUI;
    private void Start()
    {
        currentAliensPerWave = initialAliensPerWave;
        GlobalReferences.instance.waveNumber = currentWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentAliensAlive.Clear();
        currentWave++;

        GlobalReferences.instance.waveNumber = currentWave;
        currentWaveUI.text = "Wave: " + currentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0 ; i < currentAliensPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var alien = Instantiate(alienPrefab, spawnPosition, Quaternion.identity);

            Enemies enemyScript = alien.GetComponent<Enemies>();
            
            currentAliensAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        List<Enemies> aliensToRemove = new List<Enemies>();
        foreach (Enemies alien in currentAliensAlive)
        {
            if (alien.isDead)
            {
                aliensToRemove.Add(alien);
            }
        }

        foreach (Enemies alien in aliensToRemove)
        {
            currentAliensAlive.Remove(alien);
        }
        
        aliensToRemove.Clear();

        if (currentAliensAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            coolDownCounter -= Time.deltaTime;
        }
        else
        {
            coolDownCounter = waveCooldown;
        }

        coolDownCounterUI.text = coolDownCounter.ToString("F0");
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);

        currentAliensPerWave *= 2;
        StartNextWave();
    }
}
