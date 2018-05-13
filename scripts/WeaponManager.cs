using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {



    public GameObject activeWeapon;
    public float speed = 1.5f;
    private Vector3 target;
    private bool init = false;

    ThrowingWeapon wpn;

	// Use this for initialization
    /*
	void Start () {
        if (activeWeapon != null)
        {
            initialize();
        }
    }*/

    public void initialize() {
        if (init == false)
        {
            this.wpn = activeWeapon.GetComponent<ThrowingWeapon>();
            GetComponent<SpriteRenderer>().sprite = this.wpn.sprite;
            init = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5;
            GameObject playerCamera = transform.parent.GetComponent<PlayerController>().plCam;

            Vector3 mouseWorld = playerCamera.GetComponent<Camera>().ScreenToWorldPoint(mousePos);

            Vector2 direction = (Vector2)((mouseWorld - transform.position));
            direction.Normalize();
            fire(direction);
        }
    }

    [PunRPC]
    private void fire(Vector3 direction) {
        PhotonView photonView = this.GetComponent<PhotonView>();

        // strange things happen if wpn for some reason is null before attack
        if (wpn.projectile != null)
        {
            // Creates the bullet locally
            GameObject bullet = (GameObject)Instantiate(
                                    wpn.projectile,
                                    transform.position + (Vector3)(direction * 0.5f),
                                    Quaternion.identity);

            // Adds velocity to the bullet
            bullet.GetComponent<Rigidbody2D>().velocity = direction * wpn.projectileSpeed;

            if (photonView.isMine)
            {

                photonView.RPC("fire", PhotonTargets.OthersBuffered, direction);
            }
            // TODO this might be needed in the future
            /*
            else
            {
                // smooth multiplayer movement 
                smoothNetMovement();
            }*/
        }
    }

    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 8);
    }

    // Called by PUN several times per second, so that your script can write and read synchronization data for the PhotonView.
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            target = (Vector3)stream.ReceiveNext();
        }
    }
}
