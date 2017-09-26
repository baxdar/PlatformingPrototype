using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    SpriteRenderer spriterenderer;
    public int damage;
    public float lastTimer;

    void Start() {
        spriterenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, lastTimer);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Platforms")
        Destroy(gameObject);
    }
}
