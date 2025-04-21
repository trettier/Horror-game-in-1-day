using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IFieldOfView
{

    event Action<Vector2> playerFound;

    void DetectTargets(Vector2 currentDirection);

}