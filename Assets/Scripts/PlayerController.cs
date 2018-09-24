using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 0.5f;

	void Update ()
    {
        //TODO: Add object rotation with what direction the player is moving the character

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        this.transform.position = new Vector3(transform.position.x + (x * speed), transform.position.y, transform.position.z + (z * speed));
	}
}
