using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionInitiation : MonoBehaviour
{

    public string ip = "127.0.0.1";
    public int port = 25000;

    private string errorMessage;
    private bool connecting = false;

    void OnGUI()
    {
        //if the player is NOT connected
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            ip = GUI.TextField(new Rect(110, 10, 100, 25), ip);
            port = int.Parse(GUI.TextField(new Rect(110, 35, 100, 25), "" + port));
            GUI.Label(new Rect(215, 10, 400, 25), errorMessage);

            if (GUI.Button(new Rect(10, 10, 100, 25), "Start Client"))
            {
                connecting = true;
                errorMessage = "";
                Network.Connect(ip, port);
            }

            if (GUI.Button(new Rect(10, 35, 100, 25), "Create Server"))
            {
                Network.InitializeServer(1, port, false);
            }

            if (connecting)
            {
                GUI.Label(new Rect(215, 10, 300, 25), "Connecting..");
            }
        }
        else //if the player IS connected
        {
            connecting = false;
            if(Network.peerType == NetworkPeerType.Server && Network.connections.Length > 0)
            {
                SceneManager.LoadScene(1);
            }
            else if (Network.peerType == NetworkPeerType.Client)
            {
                SceneManager.LoadScene(1);
            }
            GUI.Label(new Rect(10, 35, 300, 25), "Waiting for other player to connect..");
        }

    }

    void OnFailedToConnect(NetworkConnectionError error)
    {
        connecting = false;
        if (error == NetworkConnectionError.ConnectionFailed)
        {
            errorMessage = "Failed to Connect!";
        }
        else if (error == NetworkConnectionError.TooManyConnectedPlayers)
        {
            errorMessage = "Sorry! This host is already playing with someone.";
        }
        else {
            errorMessage = error.ToString();
        }
    }
}