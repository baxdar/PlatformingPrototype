using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public GameObject player;

    void Update() {
        if (player != null) {
            Vector3 playerPosition = player.transform.position;
            Vector3 cam = gameObject.transform.position;
            Vector3 distanceToTravel = playerPosition - cam;
            distanceToTravel *= .1f;
            gameObject.transform.position += distanceToTravel;
        }
    }
}
