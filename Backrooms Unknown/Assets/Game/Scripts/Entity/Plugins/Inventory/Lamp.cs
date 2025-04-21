using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Lamp : MonoBehaviour, IActivateItem
{
    [SerializeField] private Light2D _light2D;

    public void Activate()
    {
        _light2D.enabled = true;
    }

    public void Deactivate()
    {
        _light2D.enabled = false;
    }
}