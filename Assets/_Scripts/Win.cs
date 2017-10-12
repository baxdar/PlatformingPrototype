using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour {
    public GameObject WinGUI;    

    private void OnTriggerEnter2D(Collider2D collision) {
        WinGUI.SetActive(true);
        Destroy(gameObject);
    }
}
