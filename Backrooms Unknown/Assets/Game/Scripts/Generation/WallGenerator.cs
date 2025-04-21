using UnityEngine;
using UnityEngine.Tilemaps;

public class WallGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _roomTilemap;
    [SerializeField] private Tilemap _wallTilemap;
    [SerializeField] private Tilemap _ceilingTilemap;
    [SerializeField] private TileBase _wallTile;
    [SerializeField] private TileBase _ceilingTile;
    [SerializeField] private int _tileScale = 2;

    public void GenerateWalls()
    {
        // ������� ���������� �����
        _wallTilemap.ClearAllTiles();

        // �������� ������� ���� ������
        BoundsInt bounds = _roomTilemap.cellBounds;

        // ��������� ������� �� 1 ������ �� ��� ������� ��� �������� ����
        bounds.xMin -= 1;
        bounds.yMin -= 1;
        bounds.xMax += 1;
        bounds.yMax += 1;

        // �������� �� ���� ������� � ����������� ��������
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // ���� � ���� ������� ��� ���� ���� �������/�������� - ����������
                if (_roomTilemap.HasTile(tilePosition))
                    continue;

                // ��������� ���� 8 �������
                bool hasNeighbor = false;

                // ��������� ���� ������� (������� ���������)
                for (int nx = -1; nx <= 1; nx++)
                {
                    for (int ny = -1; ny <= 1; ny++)
                    {
                        // ���������� ���� ����������� ������
                        if (nx == 0 && ny == 0)
                            continue;

                        Vector3Int neighborPos = tilePosition + new Vector3Int(nx, ny, 0);
                        if (_roomTilemap.HasTile(neighborPos))
                        {
                            hasNeighbor = true;
                            break;
                        }
                    }
                    if (hasNeighbor) break;
                }

                // ���� ���� ����� - ������ �����
                if (hasNeighbor)
                {
                    // ��������� ������� ������
                    Vector3Int scaledPosition = new Vector3Int(
                        tilePosition.x * _tileScale,
                        tilePosition.y * _tileScale,
                        0);

                    for (int dx = 0; dx < _tileScale; dx++)
                    {
                        for (int dy = 0; dy < _tileScale; dy++)
                        {
                            _wallTilemap.SetTile(scaledPosition + new Vector3Int(dx, dy, 0), _wallTile);
                            _ceilingTilemap.SetTile(scaledPosition + new Vector3Int(dx, dy, 0), _ceilingTile);
                        }
                    }
                }
            }
        }
    }
}