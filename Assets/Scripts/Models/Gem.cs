using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    #region Properties
    [SerializeField] protected GemData data;
    public int Id { get { return data.id; } }
    public int Points { get { return data.points; } }
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;
    }
    #endregion
    
}
