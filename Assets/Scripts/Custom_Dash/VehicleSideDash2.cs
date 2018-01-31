using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSideDash : SideDash
{

    //TODO List

    //Imp Jump

    //Fixe add velocity to current / not only set with dash vector

    //coldown

    private Rigidbody vehicleRigidbody;
    [SerializeField]
    private AnimationCurve a = new AnimationCurve(new Keyframe(0, 0),new Keyframe(0.5f, 3), new Keyframe(1, 1), new Keyframe(10, 1));


    // Use this for initialization
    private void Start()
    {
        base.Initialization();
        this.vehicleRigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.A) && base.available)
            this.Jump(Direction.left);
        if (Input.GetKeyDown(KeyCode.E) && base.available)
            this.Jump(Direction.right);
    }

    protected override void Jump(Direction direction)
    {
        Vector2 angle = MathsTools.DegreesToVector2(base.dashAngleInDegrees);
        Vector3 oldSpeed = this.vehicleRigidbody.velocity;
        this.vehicleRigidbody.velocity = transform.TransformVector(new Vector3(angle.x * (int)direction, -angle.y) * base.speed)
                                        + oldSpeed;
        this.vehicleRigidbody.useGravity = false;
        StartCoroutine(AutoStop(direction, oldSpeed));
        StartCoroutine(Cooldown());
    }

    private IEnumerator AutoStop(Direction direction, Vector3 oldSpeed)
    {
        yield return new WaitForSeconds(base.duration);
        this.vehicleRigidbody.velocity = oldSpeed;
        this.vehicleRigidbody.useGravity = true;
    }

    private IEnumerator Cooldown()
    {
        base.available = false;
        yield return new WaitForSeconds(base.cooldown);
        base.available = true;
    }

    private void OnValidate()
    {
        base.ValidateAttributes();
    }
}
