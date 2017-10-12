using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _CharacterController : MonoBehaviour {

    private class CharacterStats {
        public class CharStat {
            private int baseStat;
            private int currentStat;
            private int minValue;
            private int maxValue;

            public int CurrentStat {
                get { return currentStat; }
                set {
                    if (value < minValue)
                        currentStat = minValue;
                    else if (value > maxValue)
                        currentStat = maxValue;
                    else
                        currentStat = value;
                }
            }

            public CharStat (int BaseStat, int MinimumValue, int MaximumValue) {
                baseStat = BaseStat;
                currentStat = baseStat;
                minValue = MinimumValue;
                maxValue = MaximumValue;
            }

            public void resetStat() {
                CurrentStat = baseStat;
            }

        }
        //These values probably need some balance;
        public int selectedStat = 1;
        public CharStat movespeed = new CharStat(6, 3, 9);
        public CharStat jumpheight = new CharStat(7, 5, 9);
        public CharStat meleeatkstr = new CharStat(3, 1, 5);
        public CharStat rangedatkstr = new CharStat(3, 1, 5);

        private void changestat(ref CharStat stat) {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                stat.CurrentStat++;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                stat.CurrentStat--;
            else if (Input.GetButtonDown("Fire3"))
                stat.resetStat();
        }

        public void selectStat() {
            switch (selectedStat) {
                case 1://movespeed
                    changestat(ref movespeed);
                    break;
                case 2://jumpheight
                    changestat(ref jumpheight);
                    break;
                case 3://meleeatk
                    changestat(ref meleeatkstr);
                    break;
                case 4://rangedatk
                    changestat(ref rangedatkstr);
                    break;
            }
        }
    }

    private CharacterStats charStats = new CharacterStats();
    private Rigidbody2D charRigidBody;
    private BoxCollider2D hitbox;
    public GameObject swipe;
    public GameObject shot;
    public LayerMask layermask;
    public Slider[] trackers = new Slider[5];
    public GameObject DeathGUI;
    private int energy;
    private Vector2 movement;
    private bool facingLeft = false;
    private bool tryJump = false;
    private bool atkCooldown = false;

    private IEnumerator attackCooldown() {
        yield return new WaitForSeconds(.5f);
        atkCooldown = false;
    }

    private void meleeAttack() {
        atkCooldown = true;
        Vector2 temporigin = new Vector2 (transform.position.x + .5f * transform.localScale.x,
                                          transform.position.y);
        Swipe tempatk = Instantiate(swipe, temporigin, new Quaternion()).GetComponent<Swipe>();
        if (facingLeft)
            tempatk.transform.localScale = new Vector2(-1, 1);
        tempatk.damage = charStats.meleeatkstr.CurrentStat;
        energy -= charStats.meleeatkstr.CurrentStat;
    }

    private void rangedAttack() {
        atkCooldown = true;
        Vector2 temporigin = new Vector2(transform.position.x + .5f * transform.localScale.normalized.x, 
                                         transform.position.y);
        Laser tempatk = Instantiate(shot, temporigin, new Quaternion()).GetComponent<Laser>();
        Rigidbody2D temp = tempatk.GetComponent<Rigidbody2D>();
        if (facingLeft)
            temp.velocity = new Vector2(-10, 0);
        else
            temp.velocity = new Vector2(10, 0);
        tempatk.damage = charStats.meleeatkstr.CurrentStat;
        energy -= (charStats.rangedatkstr.CurrentStat + 3);
    }

    private void Flip() {
        facingLeft = !facingLeft;
        Vector2 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    private bool isGrounded() {
        Vector2 castOrigin = transform.position;
        castOrigin.y -= hitbox.size.y/2;
        Vector2 castSize = new Vector2(hitbox.size.x, .05f);
        if (Physics2D.BoxCast(castOrigin, castSize, 0, Vector2.down, 0, layermask).collider != null)
            return true;
        return false;
    }

    private void calculateMovement() {//movement should autoadapt to changes
        movement.x = Input.GetAxis("Horizontal") * charStats.movespeed.CurrentStat;
        if (isGrounded()) {
            if (tryJump)
                movement.y = charStats.jumpheight.CurrentStat;
            else
                movement.y = 0;
        }
        else
            movement.y -= 9.8f * Time.fixedDeltaTime;
    }

    private void Die() {
        DeathGUI.SetActive(true);
        Destroy(gameObject);
    }

    private void Awake() {//initialization
        hitbox = GetComponent<BoxCollider2D>();
        charRigidBody = GetComponent<Rigidbody2D>();
        energy = 1000;
    }

    void Update() {//all input methods go here
        if (isGrounded())
            tryJump = false;
        if (Input.GetButton("Jump")) {
            tryJump = true;
            if (isGrounded())
                energy -= charStats.jumpheight.CurrentStat;
        }
        calculateMovement();
        if (movement.x < 0 && !facingLeft) {
            Flip();
        }
        if (movement.x > 0 && facingLeft) {
            Flip();
        }
        if (Input.GetButton("Fire1") && !atkCooldown) {
            StartCoroutine(attackCooldown());
            meleeAttack();
        }
        if (Input.GetButton("Fire2") && !atkCooldown) {
            StartCoroutine(attackCooldown());
            rangedAttack();
        }

        //probably really bad
        if (Input.GetKeyDown(KeyCode.Alpha1))
            charStats.selectedStat = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            charStats.selectedStat = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            charStats.selectedStat = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            charStats.selectedStat = 4;
        charStats.selectStat();
        if (energy <= 0) {
            Die();
        }
    }

    void FixedUpdate () {//all movement goes here
        charRigidBody.velocity = movement;
	}

    void OnGUI() {
        trackers[0].value = charStats.movespeed.CurrentStat;
        trackers[1].value = charStats.jumpheight.CurrentStat;
        trackers[2].value = charStats.meleeatkstr.CurrentStat;
        trackers[3].value = charStats.rangedatkstr.CurrentStat;
        trackers[4].value = energy;
        //for (int i = 0; i < 4; i++) {
        //    ColorBlock cb = new ColorBlock();
        //    if (charStats.selectedStat - 1 == i)
        //        cb.normalColor = Color.blue;
        //    else
        //        cb.normalColor = Color.white;
        //    trackers[1].colors = cb;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 11) {
            energy -= collision.gameObject.GetComponent<Enemy>().damage;
        }
    }
}