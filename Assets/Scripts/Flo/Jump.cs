using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float speed;
    public float dashAngleInDegrees;
    public float cooldown;

    private bool available;
    private Rigidbody vehicleRigidbody;

    private void ValidateAttributes()
    {
        this.dashAngleInDegrees = Mathf.Clamp(this.dashAngleInDegrees, 0f, 90f);
        this.speed = Mathf.Max(this.speed, 0f);
        this.cooldown = Mathf.Max(this.cooldown, 0f);
    }

    // Use this for initialization
    void Start ()
    {
        this.Initialization();
        this.vehicleRigidbody = this.GetComponent<Rigidbody>();
    }
	
	
	void FixedUpdate ()
    {
        if ((Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.LeftControl)) && this.available)
            this.Execute();
    }
    
    private void Initialization()
    {
        this.available = true;
    }

    private void Execute()
    {
        Vector2 angle = MathsTools.DegreesToVector2(this.dashAngleInDegrees);
        Vector3 oldSpeed = this.vehicleRigidbody.velocity;
        this.vehicleRigidbody.velocity = transform.TransformVector(new Vector3(0f, -angle.y, angle.x) * this.speed)
                                        + oldSpeed;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        this.available = false;
        yield return new WaitForSeconds(this.cooldown);
        this.available = true;
    }
}
