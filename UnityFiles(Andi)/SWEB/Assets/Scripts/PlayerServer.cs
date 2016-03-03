using UnityEngine;
using UnityEngine.UI;

public class PlayerServer : MonoBehaviour {

    Player player_;

    void Start()
    {
        player_ = GetComponent<Player>();
        Transform field = GetComponentInChildren<Transform>().Find("Field");
        Transform hand = GetComponentInChildren<Transform>().Find("Hand");
        if (Network.peerType == NetworkPeerType.Server)
        {
            field.gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.4f);
            hand.gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.4f);
        }
        else
        {
            GetComponentInChildren<Button>().enabled = false;
            field.gameObject.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.4f);
            hand.gameObject.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.4f);
        }
    }

    void OnGUI()
    {
        //if (Network.peerType == NetworkPeerType.Server)
            //GUI.Label(new Rect(10, 400, 200, 25), "Cards in Deck: " + player_.GetDeck().cards_.Count);
    }
}