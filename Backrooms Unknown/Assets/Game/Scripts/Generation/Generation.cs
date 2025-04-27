using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static DelaunayTriangulation;
using static Prim;
using UnityEngine.Tilemaps;

using Mirror;
using UnityEngine.UIElements;


public class Generation : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private int _roomsCount;
    [SerializeField] private int meanWidth = 10;
    [SerializeField] private int stdDevWidth = 2;
    [SerializeField] private int meanHeight = 8;
    [SerializeField] private int stdDevHeight = 2;
    [SerializeField] private int _radius;

    [Header("Tilemap References")]
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _roomTile;
    [SerializeField] private TileBase _hallwayTile;
    [SerializeField] private int _tileScale = 2;

    //[Header("Dependencies")]
    //[SerializeField] private NetworkTilemapSyncer _tilemapSyncer;

    private List<Room> _rooms;
    private List<Edge> _minimumSpanningTree;
    private List<Edge> _selectedEdges;
    private System.Random _random = new System.Random();

    public List<Vector3Int> coordList = new List<Vector3Int>();
    public Vector2 playersSpawnPoint = Vector2.zero;

    private bool _isGenerated = false;

    class Room
    {
        public RectInt bounds;
        public Vector2 center => bounds.center;

        public Room(Vector2Int location, Vector2Int size)
        {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b)
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }
    }

    public void OnStartServer()
    {
        Debug.Log($"Generation started");
        Generate();

    }


    public void ClientReceiveMap(List<Vector3Int> coords)
    {
        Debug.Log($"[Client] Received {coords.Count} tiles from server");

        foreach (var coord in coords)
        {
            for (int dx = 0; dx < _tileScale; dx++)
            {
                for (int dy = 0; dy < _tileScale; dy++)
                {
                    _tilemap.SetTile(coord + new Vector3Int(dx, dy, 0), _hallwayTile);
                }
            }
        }

        GetComponent<WallGenerator>().GenerateWalls();
    }

    void Generate()
    {
        _rooms = new List<Room>();
        _minimumSpanningTree = new List<Edge>();
        _selectedEdges = new List<Edge>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
        GetComponent<WallGenerator>().GenerateWalls();
        _isGenerated = true;
    }

    void PlaceRooms()
    {
        for (int i = 0; i < _roomsCount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Mathf.Sqrt(Random.Range(0f, 1f)) * _radius;
            Vector2Int position = new Vector2Int(
                Mathf.RoundToInt(Mathf.Cos(angle) * radius),
                Mathf.RoundToInt(Mathf.Sin(angle) * radius));

            Vector2Int size = new Vector2Int(
                Mathf.Max(3, NextGaussian(meanWidth, stdDevWidth)),
                Mathf.Max(3, NextGaussian(meanHeight, stdDevHeight)));

            Room newRoom = new Room(position, size);
            RectInt newBounds = newRoom.bounds;
            RectInt bufferBounds = new RectInt(newBounds.xMin - 1, newBounds.yMin - 1, newBounds.width + 2, newBounds.height + 2);
            Room buffer = new Room(bufferBounds.position, bufferBounds.size);

            bool add = true;
            foreach (var room in _rooms)
            {
                if (Room.Intersect(room, buffer))
                {
                    add = false;
                    break;
                }
            }

            if (add)
            {
                _rooms.Add(newRoom);
                PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);
            }
        }
    }

    void Triangulate()
    {
        Debug.Log($"Room count: {_rooms.Count}");
        List<Vector2> roomCenters = _rooms.Select(r => new Vector2(
                                                        r.bounds.center.x,
                                                        r.bounds.center.y)).ToList();


        playersSpawnPoint = roomCenters[0];
        // Get Delaunay triangles
        List<DelaunayTriangulation.Triangle> delaunay = DelaunayTriangulation.BowyerWatson(roomCenters);

        // Convert triangles to edges
        List<Edge> allEdges = new List<Edge>();
        HashSet<string> uniqueEdges = new HashSet<string>();

        foreach (var triangle in delaunay)
        {
            AddUniqueEdge(allEdges, uniqueEdges, triangle.a, triangle.b);
            AddUniqueEdge(allEdges, uniqueEdges, triangle.b, triangle.c);
            AddUniqueEdge(allEdges, uniqueEdges, triangle.c, triangle.a);
        }

        // Create MST from all edges
        _minimumSpanningTree = Prim.CreateMST(allEdges);

        // Draw MST edges
        //foreach (var edge in _minimumSpanningTree)
        //{
        //    CreateLine(edge.vertex1, edge.vertex2, Color.green);
        //}
    }

    void AddUniqueEdge(List<Edge> edges, HashSet<string> uniqueEdges, Vector2 a, Vector2 b)
    {
        // Create a unique key for the edge (order-independent)
        string key = a.x < b.x ? $"{a.x},{a.y}-{b.x},{b.y}" : $"{b.x},{b.y}-{a.x},{a.y}";

        if (!uniqueEdges.Contains(key))
        {
            edges.Add(new Edge(a, b));
            uniqueEdges.Add(key);
        }
    }

    void CreateHallways()
    {
        // Add some random edges back (15% chance)
        List<Edge> allEdges = GetAllEdgesFromTriangulation();
        foreach (var edge in allEdges)
        {
            if (!_minimumSpanningTree.Contains(edge) && _random.NextDouble() < 0.2f)
            {
                _selectedEdges.Add(edge);
                //CreateLine(edge.vertex1, edge.vertex2, Color.blue);
            }
        }

        // Combine MST with selected edges
        _selectedEdges.AddRange(_minimumSpanningTree);
    }

    void PathfindHallways()
    {
        foreach (var edge in _selectedEdges)
        {
            Vector2Int start = Vector2Int.RoundToInt(edge.vertex1);
            Vector2Int end = Vector2Int.RoundToInt(edge.vertex2);
            List<Vector2Int> corridorTiles = new List<Vector2Int>();

            Vector2Int current = start;
            corridorTiles.Add(current);

            bool horizontalFirst = Random.value > 0.5f;

            if (horizontalFirst)
            {
                while (current.x != end.x)
                {
                    current.x += (end.x > current.x) ? 1 : -1;
                    corridorTiles.Add(current);
                }
                while (current.y != end.y)
                {
                    current.y += (end.y > current.y) ? 1 : -1;
                    corridorTiles.Add(current);
                }
            }
            else
            {
                while (current.y != end.y)
                {
                    current.y += (end.y > current.y) ? 1 : -1;
                    corridorTiles.Add(current);
                }
                while (current.x != end.x)
                {
                    current.x += (end.x > current.x) ? 1 : -1;
                    corridorTiles.Add(current);
                }
            }

            foreach (var tile in corridorTiles)
            {
                PlaceHallway(tile, Vector2.one);
            }
        }
    }



    List<Edge> GetAllEdgesFromTriangulation()
    {
        List<Vector2> roomCenters = _rooms.Select(r => r.center).ToList();
        List<DelaunayTriangulation.Triangle> delaunay = DelaunayTriangulation.BowyerWatson(roomCenters);

        List<Edge> edges = new List<Edge>();
        HashSet<string> addedEdges = new HashSet<string>();

        foreach (var triangle in delaunay)
        {
            AddUniqueEdge(edges, addedEdges, triangle.a, triangle.b);
            AddUniqueEdge(edges, addedEdges, triangle.b, triangle.c);
            AddUniqueEdge(edges, addedEdges, triangle.c, triangle.a);
        }

        return edges;
    }


    void CreateLine(Vector2 start, Vector2 end, Color color)
    {
        GameObject lineObj = new GameObject("Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, new Vector3(start.x, start.y, 0));
        lr.SetPosition(1, new Vector3(end.x, end.y, 0));

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
    }

    //void PlaceCube(Vector2Int location, Vector2Int size)
    //{
    //    Vector3 offset = new Vector3(size.x / 2f, size.y / 2f, 0);
    //    GameObject go = Instantiate(_cubePrefab, new Vector3(location.x, location.y, 0) + offset, Quaternion.identity);
    //    go.transform.localScale = new Vector3(size.x, size.y, 1);
    //}

    //void PlaceRoom(Vector2Int location, Vector2Int size)
    //{
    //    PlaceCube(location, size);
    //}

    //void PlaceHallway(Vector2 position, Vector2 size)
    //{
    //    GameObject go = Instantiate(_hallwayPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
    //    go.transform.localScale = new Vector3(size.x, size.y, 1);
    //}

    void PlaceRoom(Vector2Int location, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(
                    (location.x + x) * _tileScale,
                    (location.y + y) * _tileScale,
                    0);

                for (int dx = 0; dx < _tileScale; dx++)
                {
                    for (int dy = 0; dy < _tileScale; dy++)
                    {
                        coordList.Add(tilePosition);
                        _tilemap.SetTile(tilePosition + new Vector3Int(dx, dy, 0), _roomTile);
                    }
                }
            }
        }
    }

    void PlaceHallway(Vector2 position, Vector2 size)
    {
        Vector2Int pos = Vector2Int.RoundToInt(position);
        Vector3Int tilePosition = new Vector3Int(pos.x * _tileScale, pos.y * _tileScale, 0);

        for (int dx = 0; dx < _tileScale; dx++)
        {
            for (int dy = 0; dy < _tileScale; dy++)
            {
                coordList.Add(tilePosition);
                _tilemap.SetTile(tilePosition + new Vector3Int(dx, dy, 0), _hallwayTile);
            }
        }
    }



    int NextGaussian(int mean, int stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                            Mathf.Sin(2.0f * Mathf.PI * u2);
        return (int)(mean + stdDev * randStdNormal);
    }
}