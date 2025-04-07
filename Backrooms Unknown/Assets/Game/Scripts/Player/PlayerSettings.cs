using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    private PlayerController controller;
    //private SoapAttack attack;

    void Start()
    {
        controller = gameObject.GetComponent<PlayerController>();
        controller.__init__(gameObject);
        //attack = gameObject.GetComponent<SoapAttack>();
        //if (attack != null)
        //{
        //    attack.__init__(gameObject);
        //}
    }
}
