using UnityEngine;
using System.Collections;

public class testscript : MonoBehaviour {

    public GameObject uiObject; // <-- plonk ui thingy in here

	void Start () {
        
	}

	void Update () {
        bool hasTheItem = false; // <-- has item thingy
        uiObject.GetComponent<UnityEngine.UI.Text>().text = hasTheItem;
    }
}
