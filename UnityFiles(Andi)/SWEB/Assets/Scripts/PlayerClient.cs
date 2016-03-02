using UnityEngine;
using System.Collections;

public class PlayerClient : MonoBehaviour {

    Player player_;

    void Start()
    {
        player_ = GetComponent<Player>();

        if (Network.peerType == NetworkPeerType.Client)
        {
            player_.GetDeck().CreateRandomDeck();
        }
    }

    void Update()
    {
        
    }
}