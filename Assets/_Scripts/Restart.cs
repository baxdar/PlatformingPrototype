using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
    public Button button;

    private void restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start() {
        button.onClick.AddListener(restart);
    }
}
