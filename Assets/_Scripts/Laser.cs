using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    public int damage;
    public float lastTimer;

    void Start() {
        Destroy(gameObject, lastTimer);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 8)
            Destroy(gameObject);
    }
}
