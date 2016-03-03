using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionHandler : MonoBehaviour
{
    private string errorMessage;

    void OnGUI()
    {
        //if the player is NOT connected
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            GUI.Label(new Rect(400, 10, 300, 25), "LOST CONNECTION!!!");
        }
        else //if the player IS connected
        {
            if (Network.peerType == NetworkPeerType.Client)
            {
                GUI.Label(new Rect(10, 10, 100, 25), "Client");

                if (GUI.Button(new Rect(10, 30, 100, 25), "Logout"))
                {
                    Network.Disconnect(200);//the 200 is in milliseconds for the disconnect
                }
            }

            if (Network.peerType == NetworkPeerType.Server)
            {
                GUI.Label(new Rect(10, 10, 100, 25), "Server");
                GUI.Label(new Rect(10, 30, 100, 25), "Connections: " + Network.connections.Length);

                if (GUI.Button(new Rect(10, 50, 100, 25), "Logout"))
                {
                    Network.Disconnect(200); //the 200 is in milliseconds for the disconnect
                }
            }
        }//end of "if the player IS connected"

    }
}