using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSideDash : MonoBehaviour
{
    // ATTRIBUTES
    public float speed;
    public float dashAngleInDegrees;
    public float duration;
    public float cooldown;


    protected bool available;
    protected enum Direction { left = -1, right = 1 };


    private Rigidbody vehicleRigidbody;


    private void OnValidate()
    {
        this.ValidateAttributes();
    }

    // Use this for initialization
    private void Start()
    {
        this.Initialization();
    }

    private void FixedUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.A) && this.available)
            this.Jump(Direction.left);
        if (Input.GetKeyDown(KeyCode.E) && this.available)
            this.Jump(Direction.right);
    }

    private void Jump(Direction direction)
    {
        Vector2 angle = MathsTools.DegreesToVector2(this.dashAngleInDegrees);
        Vector3 oldSpeed = this.vehicleRigidbody.velocity;
        this.vehicleRigidbody.velocity = transform.TransformVector(new Vector3(angle.x * (int)direction, -angle.y) * this.speed)
                                        + oldSpeed;
        this.vehicleRigidbody.useGravity = false;
        StartCoroutine(AutoStop(direction, oldSpeed));
        StartCoroutine(Cooldown());
    }

    private IEnumerator AutoStop(Direction direction, Vector3 oldSpeed)
    {
        yield return new WaitForSeconds(this.duration);
        this.vehicleRigidbody.velocity = oldSpeed;
        this.vehicleRigidbody.useGravity = true;
    }

    private IEnumerator Cooldown()
    {
        this.available = false;
        yield return new WaitForSeconds(this.cooldown);
        this.available = true;
    }


    private void Initialization()
    {
        this.available = true;
        this.vehicleRigidbody = this.GetComponent<Rigidbody>();
    }

    private void ValidateAttributes()
    {
        this.dashAngleInDegrees = Mathf.Clamp(this.dashAngleInDegrees, 0f, 90f);
        this.speed = Mathf.Max(this.speed, 0f);
        this.duration = Mathf.Max(this.duration, 0f);
        this.cooldown = Mathf.Max(this.cooldown, 0f);
    }
}
