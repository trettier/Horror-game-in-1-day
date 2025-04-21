using UnityEngine;
using System;

public interface IInvestigating
{

    public event Action EndOfInvestigation;

    Vector2 Investigate(Vector2 direction);

    void StopInvestigating();

}