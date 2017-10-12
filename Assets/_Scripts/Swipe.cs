using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {

    SpriteRenderer spriterenderer;
    public int damage;
    public float fadetimer;

	void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, fadetimer);
	}

    void Update () {
        spriterenderer.material.color -= new Color(0f, 0f, 0f, 2f) * Time.deltaTime;
    }
}
