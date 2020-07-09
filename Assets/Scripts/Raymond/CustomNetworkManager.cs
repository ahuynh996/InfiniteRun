using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn) {

        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        player.GetComponent<PlayerControllerMirror>().playerSpritesArrayIndex = NetworkServer.connections.Count-1;
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}