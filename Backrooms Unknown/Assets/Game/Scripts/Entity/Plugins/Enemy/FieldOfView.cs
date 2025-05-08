using UnityEngine;
using System;

public class FieldOfView : MonoBehaviour, IFieldOfView
{
    [SerializeField] private float _viewDistance;
    [SerializeField] private float _viewAngle;
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private LayerMask _obstacleMask;

    private Vector2 _currentDirection;

    public event Action<Vector2> playerFound;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);

        DrawFieldOfView();
    }

    private void DrawFieldOfView()
    {
        if (_currentDirection == Vector2.zero)
            _currentDirection = transform.right;

        float halfAngle = _viewAngle / 2;
        Vector2 viewAngleLeft = Quaternion.Euler(0, 0, halfAngle) * _currentDirection * _viewDistance;
        Vector2 viewAngleRight = Quaternion.Euler(0, 0, -halfAngle) * _currentDirection * _viewDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, viewAngleLeft);
        Gizmos.DrawRay(transform.position, viewAngleRight);

        DrawViewArc();
    }

    private void DrawViewArc()
    {
        Gizmos.color = new Color(1, 1, 0, 0.1f);
        float segments = 20;
        float angleStep = _viewAngle / segments;
        float halfAngle = _viewAngle / 2;

        Vector2 prevPoint = transform.position + (Quaternion.Euler(0, 0, -halfAngle) * _currentDirection * _viewDistance);

        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + angleStep * i;
            Vector2 nextPoint = transform.position + (Quaternion.Euler(0, 0, angle) * _currentDirection * _viewDistance);

            Gizmos.DrawLine(transform.position, nextPoint);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    public void DetectTargets(Vector2 currentDirection)
    {
        _currentDirection = currentDirection;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _viewDistance);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(_targetTag))
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                float angleBetween = Vector3.Angle(currentDirection, directionToTarget);

                if (angleBetween >= _viewAngle / 2)
                {
                    continue;
                }

                RaycastHit2D hit2D = Physics2D.Raycast(transform.position, directionToTarget, _viewDistance, _obstacleMask);
                Debug.DrawRay(transform.position, directionToTarget * _viewDistance, Color.red);

                if (hit2D.collider != null && hit2D.collider.CompareTag(_targetTag))
                {
                    playerFound.Invoke(hit2D.point);
                }
            }
        }
    }
}