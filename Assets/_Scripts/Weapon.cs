using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    SpriteRenderer spriterenderer;
    private int damage;
    private float fadetimer;

    public Weapon (int Damage, float FadeTimer) {
        damage = Damage;
        fadetimer = FadeTimer;
    }

	// Use this for initialization
	void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, fadetimer);
	}
	
	// Update is called once per frame
	void Update () {
        spriterenderer.material.color -= new Color(0f, 0f, 0f, .04f) * Time.deltaTime;
    }
}
