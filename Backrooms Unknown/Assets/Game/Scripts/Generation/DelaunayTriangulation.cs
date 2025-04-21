using System.Collections.Generic;
using UnityEngine;

public static class DelaunayTriangulation
{
    public class Triangle
    {
        public Vector2 a, b, c;

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public bool ContainsVertex(Vector2 v)
        {
            return a == v || b == v || c == v;
        }

        public bool ContainsPointInCircumcircle(Vector2 point)
        {
            float ax = a.x - point.x;
            float ay = a.y - point.y;
            float bx = b.x - point.x;
            float by = b.y - point.y;
            float cx = c.x - point.x;
            float cy = c.y - point.y;

            float det = (ax * ax + ay * ay) * (bx * cy - cx * by)
                      - (bx * bx + by * by) * (ax * cy - cx * ay)
                      + (cx * cx + cy * cy) * (ax * by - bx * ay);

            // Логируем определитель для отладки
            Debug.Log($"Determinant: {det}");

            // Коррекция: Проверка с учетом погрешности
            return det > 0.0001f; // Погрешность для стабилизации
        }

        public override string ToString()
        {
            return $"Triangle: A{a}, B{b}, C{c}";
        }

        // Получаем рёбра треугольника
        public List<(Vector2, Vector2)> GetEdges()
        {
            return new List<(Vector2, Vector2)>
            {
                (a, b),
                (b, c),
                (c, a)
            };
        }
    }

    public static List<Triangle> BowyerWatson(List<Vector2> points)
    {
        List<Triangle> triangles = new List<Triangle>();

        // Логируем количество точек
        Debug.Log($"Points count: {points.Count}");
        foreach (var point in points)
        {
            Debug.Log($"Point: {point}");
        }

        // Создаём супер-треугольник, покрывающий все точки
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (var p in points)
        {
            minX = Mathf.Min(minX, p.x);
            minY = Mathf.Min(minY, p.y);
            maxX = Mathf.Max(maxX, p.x);
            maxY = Mathf.Max(maxY, p.y);
        }

        float dx = maxX - minX;
        float dy = maxY - minY;
        float deltaMax = Mathf.Max(dx, dy);
        float midX = (minX + maxX) / 2f;
        float midY = (minY + maxY) / 2f;

        Vector2 p1 = new Vector2(midX - 2 * deltaMax, midY - 2 * deltaMax);
        Vector2 p2 = new Vector2(midX + 2 * deltaMax, midY - 2 * deltaMax);
        Vector2 p3 = new Vector2(midX, midY + 2 * deltaMax);

        // Логируем супер-треугольник
        Debug.Log($"Super triangle: P1({p1}), P2({p2}), P3({p3})");

        triangles.Add(new Triangle(p1, p2, p3));

        // Основной алгоритм
        foreach (var point in points)
        {
            List<Triangle> badTriangles = new List<Triangle>();
            foreach (var triangle in triangles)
            {
                if (triangle.ContainsPointInCircumcircle(point))
                {
                    badTriangles.Add(triangle);
                }
            }

            List<(Vector2, Vector2)> polygon = new List<(Vector2, Vector2)>();

            foreach (var triangle in badTriangles)
            {
                var edges = new (Vector2, Vector2)[] {
                    (triangle.a, triangle.b),
                    (triangle.b, triangle.c),
                    (triangle.c, triangle.a)
                };

                foreach (var edge in edges)
                {
                    bool isShared = false;
                    foreach (var other in badTriangles)
                    {
                        if (other == triangle) continue;
                        if (Prim.EdgeExists(edge, other))
                        {
                            isShared = true;
                            break;
                        }
                    }

                    if (!isShared)
                    {
                        polygon.Add(edge);
                    }
                }
            }

            foreach (var triangle in badTriangles)
            {
                triangles.Remove(triangle);
            }

            foreach (var edge in polygon)
            {
                if (edge.Item1 != edge.Item2) // Добавляем проверку уникальности
                {
                    triangles.Add(new Triangle(edge.Item1, edge.Item2, point));
                }
            }
        }

        // Удаляем треугольники, связанные с супер-треугольником
        triangles.RemoveAll(t => t.ContainsVertex(p1) || t.ContainsVertex(p2) || t.ContainsVertex(p3));

        return triangles;
    }

}
