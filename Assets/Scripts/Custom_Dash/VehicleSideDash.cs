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
    [SerializeField]
    private AnimationCurve speedVariation = new AnimationCurve(new Keyframe(0f, 0.1f), new Keyframe(0.1f, 1f), new Keyframe(0.9f, 1f), new Keyframe(1f, 0f));


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
            this.Dash(Direction.left);
        if (Input.GetKeyDown(KeyCode.E) && this.available)
            this.Dash(Direction.right);
    }

    private void Dash(Direction direction)
    {
        //Vector2 angle = MathsTools.DegreesToVector2(this.dashAngleInDegrees);
        Vector3 originalSpeed = this.vehicleRigidbody.velocity;
        //this.vehicleRigidbody.useGravity = false;
        //this.vehicleRigidbody.velocity = transform.TransformVector(new Vector3(angle.x * (int)direction, -angle.y) * this.speed)
        //                                + originalSpeed;
        StartCoroutine(VarySpeedDuringDash(direction, originalSpeed));
        StartCoroutine(AutoStop(direction, originalSpeed));
        StartCoroutine(Cooldown());
    }

    private IEnumerator VarySpeedDuringDash(Direction direction, Vector3 originalSpeed)
    {
        Vector2 angle = MathsTools.DegreesToVector2(this.dashAngleInDegrees);
        float step = this.duration / 100f;
        for (float counter = 0f; counter < this.duration; counter += step)
        {
            float speedAccordingToCurve = this.speed * this.speedVariation.Evaluate(counter);
            this.vehicleRigidbody.velocity = transform.TransformVector(new Vector3(angle.x * (int)direction, -angle.y) * speedAccordingToCurve)
                                            + originalSpeed;
            yield return new WaitForSeconds(step);
        }
    }

    private IEnumerator AutoStop(Direction direction, Vector3 oldSpeed)
    {
        yield return new WaitForSeconds(this.duration);
        this.vehicleRigidbody.velocity = oldSpeed;
        //this.vehicleRigidbody.useGravity = true;
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
