using System.Collections.Generic;
using UnityEngine;
using System;

public class Investigating : MonoBehaviour, IInvestigating
{
    [SerializeField] private float _rotationTime;
    private bool _isInvestigating = false;
    private List<Vector2> _directions;
    private Vector2 _currentDirection;
    private float _timer;


    public event Action EndOfInvestigation;


    public Vector2 Investigate(Vector2 currentDirection)
    {
        currentDirection = GetDirection(currentDirection);

        if (!_isInvestigating)
        {
            _directions = new List<Vector2> { Vector2.up, Vector2.left, Vector2.down, Vector2.right };
            _isInvestigating = true;
            _timer = _rotationTime;
            _currentDirection = currentDirection;
        }
        else if (_timer <= 0)
        {
            if (_directions.Count > 0)
            {
                int index = _directions.IndexOf(_currentDirection);
                _currentDirection = _directions[(index + 1) % _directions.Count];
                _directions.RemoveAt(index);
                _timer = _rotationTime;
            }
            else
            {
                _isInvestigating = false;
                EndOfInvestigation.Invoke();
            }
        }
        else
        {
            _timer -= Time.deltaTime;
        }
        return _currentDirection;
    }

    public void StopInvestigating()
    {
        _isInvestigating = false;
    }

    Vector2 GetDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return direction.y > 0 ? Vector2.up : Vector2.down;
        }
    }
}