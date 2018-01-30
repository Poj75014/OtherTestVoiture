using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CubeGround : MonoBehaviour, IGround
{
    //private float groundDistance; //old
    private Bounds bounds;

    public bool IsGrounded()
    {
        // return Physics.Raycast(this.transform.position, Vector3.down, this.groundDistance + 0.1f); //old
        return Physics.CheckCapsule(this.bounds.center, new Vector3(this.bounds.center.x, this.bounds.min.y - 0.1f, this.bounds.center.z), 0.18f);
    }

    // Use this for initialization
    void Start ()
    {
        //this.groundDistance = this.GetComponent<Collider>().bounds.extents.y; //old
        this.bounds = this.GetComponent<Collider>().bounds;
    }
}
