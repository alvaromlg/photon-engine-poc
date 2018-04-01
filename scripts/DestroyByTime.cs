using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    public int secondsToDestroy;

    private void Awake()
    {
        Destroy(this.gameObject, secondsToDestroy);
    }
}
