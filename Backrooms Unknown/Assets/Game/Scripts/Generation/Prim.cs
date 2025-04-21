using System.Collections.Generic;
using UnityEngine;

public class Prim
{
    public static bool EdgeExists((Vector2, Vector2) edge, DelaunayTriangulation.Triangle triangle)
    {
        return (IsSameEdge(edge, (triangle.a, triangle.b)) ||
                IsSameEdge(edge, (triangle.b, triangle.c)) ||
                IsSameEdge(edge, (triangle.c, triangle.a)));
    }

    private static bool IsSameEdge((Vector2, Vector2) e1, (Vector2, Vector2) e2)
    {
        return (e1.Item1 == e2.Item1 && e1.Item2 == e2.Item2) ||
               (e1.Item1 == e2.Item2 && e1.Item2 == e2.Item1);
    }

    public class Edge
    {
        public Vector2 vertex1, vertex2;
        public float weight;

        public Edge(Vector2 v1, Vector2 v2)
        {
            vertex1 = v1;
            vertex2 = v2;
            weight = Vector2.Distance(v1, v2);
        }
    }

    public static List<Edge> CreateMST(List<Edge> allEdges)
    {
        List<Edge> mst = new List<Edge>();
        HashSet<Vector2> visitedVertices = new HashSet<Vector2>();

        if (allEdges.Count == 0) return mst;

        // Start with first vertex of first edge
        Vector2 startVertex = allEdges[0].vertex1;
        visitedVertices.Add(startVertex);

        // Priority queue
        List<Edge> candidateEdges = new List<Edge>();

        // Add all edges connected to start vertex
        foreach (var edge in allEdges)
        {
            if (edge.vertex1 == startVertex || edge.vertex2 == startVertex)
            {
                candidateEdges.Add(edge);
            }
        }

        while (candidateEdges.Count > 0)
        {
            // Sort and get smallest edge
            candidateEdges.Sort((e1, e2) => e1.weight.CompareTo(e2.weight));
            Edge smallest = candidateEdges[0];
            candidateEdges.RemoveAt(0);

            Vector2 next = visitedVertices.Contains(smallest.vertex1) ?
                          smallest.vertex2 : smallest.vertex1;

            if (visitedVertices.Contains(next))
                continue;

            visitedVertices.Add(next);
            mst.Add(smallest);

            // Add new edges from next vertex
            foreach (var edge in allEdges)
            {
                if ((edge.vertex1 == next && !visitedVertices.Contains(edge.vertex2)) ||
                    (edge.vertex2 == next && !visitedVertices.Contains(edge.vertex1)))
                {
                    candidateEdges.Add(edge);
                }
            }
        }

        return mst;
    }
}
