using Mirror;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;

public class NetworkTilemapSyncer : NetworkBehaviour
{
    [SerializeField] private Generation levelGenerator;
    [SerializeField] private bool DebugOn;

    [TargetRpc]
    public void GenerateMap(NetworkConnectionToClient target, List<Vector3Int> coordList)
    {
        Debug.Log("Put coordList in generator");
        levelGenerator.ClientReceiveMap(coordList);
    }


    [TargetRpc]
    public void DebugMap(NetworkConnectionToClient target, List<DebugLine> debugLines)
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
