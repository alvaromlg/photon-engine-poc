using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {



    public GameObject activeWeapon;
    ThrowingWeapon wpn;

	// Use this for initialization
	void Start () {
        wpn = activeWeapon.GetComponent<ThrowingWeapon>();

        GetComponent<SpriteRenderer>().sprite = wpn.sprite;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("X: " + transform.parent.localScale.x);
            Vector3 rotation = transform.parent.localScale.x == 1 ? Vector3.zero : Vector3.forward * 180;
            GameObject projectile = (GameObject)Instantiate(wpn.projectile, transform.position, Quaternion.Euler(rotation));

            if (wpn.projectileMode == ThrowingWeapon.Modes.Straight)
            {
                projectile.GetComponent<Rigidbody2D>().velocity = transform.parent.localScale.x * Vector2.right * wpn.projectileSpeed;
            }
        }
	}
}
