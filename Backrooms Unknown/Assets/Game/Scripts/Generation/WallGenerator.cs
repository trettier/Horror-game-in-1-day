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
        // Очищаем предыдущие стены
        _wallTilemap.ClearAllTiles();

        // Получаем границы всех тайлов
        BoundsInt bounds = _roomTilemap.cellBounds;

        // Расширяем границы на 1 клетку во все стороны для проверки стен
        bounds.xMin -= 1;
        bounds.yMin -= 1;
        bounds.xMax += 1;
        bounds.yMax += 1;

        // Проходим по всем клеткам в расширенных границах
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Если в этой позиции уже есть тайл комнаты/коридора - пропускаем
                if (_roomTilemap.HasTile(tilePosition))
                    continue;

                // Проверяем всех 8 соседей
                bool hasNeighbor = false;

                // Проверяем всех соседей (включая диагонали)
                for (int nx = -1; nx <= 1; nx++)
                {
                    for (int ny = -1; ny <= 1; ny++)
                    {
                        // Пропускаем саму центральную клетку
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

                // Если есть сосед - ставим стену
                if (hasNeighbor)
                {
                    // Учитываем масштаб тайлов
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