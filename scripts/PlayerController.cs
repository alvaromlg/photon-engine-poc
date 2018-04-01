using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	/// <summary>
	/// Player controller which is observed by photon view
    /// </summary>

    public float moveSpeed;
    // player camara, this camara is nested inside the player's prefab
    public GameObject plCam;
    // photon view script observing itself (PlayerController script)
    public PhotonView photonView;

    // animator, 2d sprite
    private Animator anim;
    private Vector3 selfPos;
    private Rigidbody2D myPlayerRigidbody2D;

    private void Awake()
    {
        if (photonView.isMine)
        {
            anim = GetComponent<Animator>();
            myPlayerRigidbody2D = GetComponent<Rigidbody2D>();

            // activate player camera
            plCam.SetActive(true);

            // set player collisions to dynamic so it can collide with kinematic objects
            myPlayerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // if it's me (dont forget we have multiple player instances) updates position/sprite
        if (photonView.isMine)
        {
            checkInput();
        }
        else
        {
            // smooth multiplayer movement 
            smoothNetMovement();
        }
    }

    // updates transform and animator
    private void checkInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float moveHorizontal = horizontal * moveSpeed * Time.deltaTime;
        float moveVertical = vertical * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveHorizontal, moveVertical));

        anim.SetFloat("MoveX", horizontal);
        anim.SetFloat("MoveY", vertical);
    }

    private void smoothNetMovement() {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
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
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }
}
