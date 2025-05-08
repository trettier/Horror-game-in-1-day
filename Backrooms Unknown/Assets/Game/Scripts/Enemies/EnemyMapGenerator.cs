using Mirror;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;
public class EnemyMapGenerator : NetworkBehaviour
{
    [SerializeField] private Generation levelGenerator;

    [SerializeField] private PatrolPoint patrolPointPrefab; // префаб PatrolPoint
    private Dictionary<Vector2, PatrolPoint> placedPoints = new Dictionary<Vector2, PatrolPoint>();
    [SerializeField] private bool DebugOn;
    private List<PatrolPoint> patrolPointsList;
    public PatrolPoint randomPoint;

    public void GeneratePatrolPoints(List<DebugLine> debugLines)
    {
        placedPoints.Clear();

        foreach (var line in debugLines)
        {
            var startPoint = GetOrCreatePatrolPoint(line.start);
            var endPoint = GetOrCreatePatrolPoint(line.end);

            // Связываем точки
            if (!startPoint.patrolObjects.Contains(endPoint))
                startPoint.patrolObjects.Add(endPoint);

            if (!endPoint.patrolObjects.Contains(startPoint))
                endPoint.patrolObjects.Add(startPoint);
        }
        patrolPointsList = new List<PatrolPoint>(placedPoints.Values);
        randomPoint = patrolPointsList[Random.Range(0, patrolPointsList.Count)];
    }

    private PatrolPoint GetOrCreatePatrolPoint(Vector2 position)
    {
        if (placedPoints.TryGetValue(position, out var existingPoint))
        {
            return existingPoint;
        }

        var newPoint = Instantiate(patrolPointPrefab, position, Quaternion.identity);
        placedPoints.Add(position, newPoint);
        return newPoint;
    }

    public void DebugMap(List<DebugLine> debugLines)
    {
        if (DebugOn)
        {
            Debug.Log($"Paint debug lines, their count: {debugLines.Count}");
            foreach (var line in debugLines)
            {
                levelGenerator.CreateLine(line.start, line.end, Color.green);
            }
        }
    }
}
