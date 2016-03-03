using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inspector : MonoBehaviour {

    Text card_name_;
    Text card_description_;

    // Use this for initialization
    void Start () {
        card_name_ = GetComponentsInChildren<Text>()[0];
        card_description_ = GetComponentsInChildren<Text>()[1];
        RectTransform rec_trans = GetComponent<RectTransform>();
       // rec_trans.sizeDelta = new Vector2(rec_trans.sizeDelta.x, Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowInspector() {
        this.gameObject.SetActive(true);
    }

    public void HideInspector() {
        this.gameObject.SetActive(false);
    }

    public void ShowCard(Card card) {
        card_name_.text = card.GetName();
        card_description_.text = card.GetDescription();
    }
}
