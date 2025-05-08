using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ItemAnimatorController : NetworkBehaviour
{
    private Material _material;


    public void Initialize(Material material)
    {
        _material = material;
    }

    public void OutlineOn()
    {
        _material.SetFloat("_OutlineOff", 0f);
    }

    public void OutlineOff()
    {
        _material.SetFloat("_OutlineOff", 1f);
    }
}