using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerIdentity : NetworkBehaviour
{
    [SyncVar] public string PlayerName;

    public override void OnStartAuthority()
    {
        if (NetworkServer.active) { PlayerName = "serverPlayer"; } else { PlayerName = "clientPlayer"; }
        CmdRegisterAtNetworkState();
    }

    [Command]
    public void CmdRegisterAtNetworkState()
    {
        FindObjectOfType<NetworkState>().RegisterPlayer(this);
    }
}
