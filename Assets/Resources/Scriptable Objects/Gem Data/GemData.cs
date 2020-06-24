using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Gem Data", menuName = "Gem Data")]
public class GemData : ScriptableObject
{
    public int id;
    public string gemName;
    public int points;
    public Sprite sprite;
}
