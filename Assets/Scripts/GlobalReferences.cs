using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
 public static GlobalReferences instance;

 public GameObject bulletImpactEffectPrefab;

 public GameObject grenadeExplosionEffect;

 public GameObject bloodSprayEffect;

 public int waveNumber;
 

 private void Awake()
 {
  if (instance != null && instance != this)
  {
    Destroy(gameObject);
  }
  else
  {
      instance = this;
  }
 }
}
