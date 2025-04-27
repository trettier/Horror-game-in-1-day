using Mirror;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;

public class NetworkTilemapSyncer : NetworkBehaviour
{
    [SerializeField] private Generation levelGenerator;

    [TargetRpc]
    public void GenerateMap(NetworkConnectionToClient target, List<Vector3Int> coordList)
    {
        Debug.Log("Put coordList in generator");
        levelGenerator.ClientReceiveMap(coordList);
    }
}
