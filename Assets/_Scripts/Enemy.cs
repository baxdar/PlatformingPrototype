using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int health;
    public int damage = 50;
    private Rigidbody2D rb;
    private Vector2 vel = new Vector2(-5, 0);

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = vel;
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 10) {
            vel *= -1;
        }
        else if (collision.gameObject.layer == 9) {
            int atkdmg = 0;
            if (collision.GetComponent<Swipe>() != null)
                atkdmg = collision.GetComponent<Swipe>().damage;
            else if (collision.GetComponent<Laser>() != null)
                atkdmg = collision.GetComponent<Laser>().damage;
            health -= atkdmg;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 12) {
            vel *= -1;
        }
    }

    void Update () {
        rb.velocity = vel;
        if (health <= 0)
            Destroy(gameObject);
	}
}
