using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Generator Block", menuName = "EnemyRandomStats")]
public class EnemyRandomStatGenerator : ScriptableObject
{
    public string enemyName;

    public int starLevel = 2;

    public int strenghtPercentage;
    public int dexterityPercentage;
    public int vitalityPercentage;
    public int wisdomPercentage;
    public int intelligencePercentage;
   


}
