using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject target;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            // camera floating at -8 axis z over the player
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -8);
            transform.rotation = target.transform.rotation;
        }
    }
}