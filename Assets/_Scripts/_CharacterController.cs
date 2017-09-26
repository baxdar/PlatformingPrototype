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
        public CharStat jumpheight = new CharStat(6, 3, 9);
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
    public LayerMask layermask;
    public Slider[] trackers = new Slider[5];
    private int energy;
    private Vector2 movement;
    private bool tryJump = false;
    private bool atkCooldown = false;

    private IEnumerator attackCooldown() {
        yield return new WaitForSeconds(.5f);
        atkCooldown = false;
    }

    private void meleeAttack() {

    } 

    private void rangedAttack() {

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
        if (Input.GetButton("Fire1") && !atkCooldown) {
            atkCooldown = true;
            StartCoroutine(attackCooldown());

            energy -= charStats.meleeatkstr.CurrentStat;
        }
        if (Input.GetButton("Fire2") && !atkCooldown) {
            atkCooldown = true;
            StartCoroutine(attackCooldown());

            energy -= (charStats.rangedatkstr.CurrentStat + 3);
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

        calculateMovement();
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
    }
}