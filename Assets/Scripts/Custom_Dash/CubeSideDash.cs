using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSideDash : SideDash
{
    //public Vector3 velocity; //debug


    // Use this for initialization
    private void Start()
    {
        base.Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        //print(this.groundProperties.IsGrounded()); //debug

        // to delete ----------------------------------------------------
        if (Input.GetKey(KeyCode.UpArrow)/* && this.groundProperties.IsGrounded()*/)
            this.transform.Translate(Vector3.forward);
        if (Input.GetKey(KeyCode.DownArrow) /*&& this.groundProperties.IsGrounded()*/)
            this.transform.Translate(Vector3.back);
        // --------------------------------------------------------------

        if (Input.GetKeyDown(KeyCode.A) /*&& this.groundProperties.IsGrounded() /*&& this.groundProperties != null*/)
            this.Jump(Direction.left);
        if (Input.GetKeyDown(KeyCode.E) /*&& this.groundProperties.IsGrounded() /*&& this.groundProperties != null*/)
            this.Jump(Direction.right);

        //velocity = this.GetComponent<Rigidbody>().velocity; //debug
    }    

    protected override void Jump(Direction direction)
    {
        Vector2 angle = MathsTools.DegreesToVector2(this.dashAngleInDegrees);
        this.GetComponent<Rigidbody>().velocity = new Vector3(angle.x * (int)direction, -angle.y) * this.speed;
    }

    private void OnValidate()
    {
        base.ValidateAttributes();
    }
}
