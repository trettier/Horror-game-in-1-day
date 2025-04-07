using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PatrolPoint : MonoBehaviour
{
    [SerializeField]
    protected List<PatrolPoint> patrolObjects;


    private void Awake()
    {

        GetComponent<Light2D>().enabled = false;

    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public PatrolPoint GetRandomPoint()
    {
        return patrolObjects[Random.Range(0, patrolObjects.Count)];
    }
}
