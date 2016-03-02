using UnityEngine;
using System.Collections;

public class PlayerServer : MonoBehaviour {

    Player player_;

    void Start()
    {
        player_ = GetComponent<Player>();

        if (Network.peerType == NetworkPeerType.Server)
        {
            player_.GetDeck().CreateRandomDeck();
        }
    }

    void Update()
    {
        
    }
}