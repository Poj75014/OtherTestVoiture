using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    public class Dash : CommonAbility
    {
        [SerializeField]
        private float _dashAngleInDegrees;
        [Tooltip("Speed percentage used depending on duration percentage elapsed")]
        [SerializeField]
        private AnimationCurve _speedVariation = new AnimationCurve(new Keyframe(0f, 1f),
                                                                    new Keyframe(0.1f, 1f),
                                                                    new Keyframe(0.9f, 1f),
                                                                    new Keyframe(1f, 0f));
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _duration;

        private Rigidbody _vehicleRigidbody;

        protected enum Direction { left = -1, right = 1 };



        private void Execute(Direction direction)
        {
            Vector3 originalSpeed = _vehicleRigidbody.velocity;
            _vehicleRigidbody.useGravity = false;

            StartCoroutine(VarySpeedDuringDash(direction, originalSpeed));
            StartCoroutine(AutoStop(direction, originalSpeed));
            StartCoroutine(base.LaunchCooldown());
        }

        private IEnumerator VarySpeedDuringDash(Direction direction, Vector3 originalSpeed)
        {
            Vector2 angle = MathsTools.DegreesToVector2(_dashAngleInDegrees);
            float start = Time.fixedTime;

            while (start + _duration > Time.fixedTime)
            {
                float speedAccordingToCurve = _speed * _speedVariation.Evaluate((Time.fixedTime - start) / _duration);
                _vehicleRigidbody.velocity = transform.TransformVector(new Vector3(angle.x * (int)direction, -angle.y) * speedAccordingToCurve)
                                                + originalSpeed;
                yield return new WaitForFixedUpdate();
            }

            /*float step = _duration / 100f;
            for (float counter = 0f; counter <= _duration; counter += step)
            {
                float speedAccordingToCurve = _speed * _speedVariation.Evaluate(counter);
                _vehicleRigidbody.velocity = transform.TransformVector(new Vector3(angle.x * (int)direction, -angle.y) * speedAccordingToCurve)
                                                + originalSpeed;
                yield retun new WaitForSeconds(step);
            }*/
        }

        private IEnumerator AutoStop(Direction direction, Vector3 originalSpeed)
        {
            yield return new WaitForSeconds(_duration);
            _vehicleRigidbody.velocity = originalSpeed;
            _vehicleRigidbody.useGravity = true;
        }

        // MONOBEHAVIOUR -----------------------------------------------------------------

        private void OnValidate()
        {
            this.ValidateAttributes();
        }

        private void Start()
        {
            this.Initialization();
        }

        private void FixedUpdate()
        {
            if (base.Available && (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.A)))
                this.Execute(Direction.left);
            if (base.Available && (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.E)))
                this.Execute(Direction.right);
        }

        // ----------------------------------------------------------------- MONOBEHAVIOUR

        protected override void ValidateAttributes()
        {
            base.ValidateAttributes();
            _dashAngleInDegrees = Mathf.Clamp(_dashAngleInDegrees, 0f, 90f);
            _speed = Mathf.Max(_speed, 0f);
            _duration = Mathf.Max(_duration, 0f);
        }

        protected override void Initialization()
        {
            base.Initialization();
            _vehicleRigidbody = this.GetComponent<Rigidbody>();
        }
    }
}