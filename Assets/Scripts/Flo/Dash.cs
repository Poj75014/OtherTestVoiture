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
        private float _minimumDuration;
        [SerializeField]
        private float _maximumDuration;
        [SerializeField]
        private float _maximumChargingDuration;
        
        private float _currentChargingDuration;
        private Direction _chosenDirection;

        private Rigidbody _vehicleRigidbody;

        protected enum Direction { left = -1, right = 1, none = 0 };



        private void Execute(Direction direction, float duration)
        {
            Vector3 originalSpeed = _vehicleRigidbody.velocity;
            _vehicleRigidbody.useGravity = false;

            StartCoroutine(VarySpeedDuringDash(direction, duration, originalSpeed));
            StartCoroutine(AutoStop(direction, duration, originalSpeed));
            StartCoroutine(base.LaunchCooldown());
        }

        private IEnumerator VarySpeedDuringDash(Direction direction, float duration, Vector3 originalSpeed)
        {
            Vector2 angle = MathsTools.DegreesToVector2(_dashAngleInDegrees);
            float start = Time.fixedTime;

            while (start + duration > Time.fixedTime)
            {
                float speedAccordingToCurve = _speed * _speedVariation.Evaluate((Time.fixedTime - start) / duration);
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

        private IEnumerator AutoStop(Direction direction, float duration, Vector3 originalSpeed)
        {
            yield return new WaitForSeconds(duration);
            _vehicleRigidbody.velocity = originalSpeed;
            _vehicleRigidbody.useGravity = true;
        }

        private IEnumerator ChargeDashPower()
        {
            _currentChargingDuration = 0f;

            while (_currentChargingDuration < _maximumChargingDuration)
            {
                yield return null;
                _currentChargingDuration += Time.deltaTime;
            }
        }

        private void StopChargingAndExecute(Direction direction)
        {
            StopCoroutine(ChargeDashPower());

            _currentChargingDuration = Mathf.Min(_currentChargingDuration, _maximumChargingDuration);
            float percentageCharged = Mathf.InverseLerp(0f, _maximumChargingDuration, _currentChargingDuration);
            float effectiveDuration = Mathf.Lerp(_minimumDuration, _maximumDuration, percentageCharged);
            print(_currentChargingDuration + "   " + percentageCharged + "   " + effectiveDuration);

            Execute(direction, effectiveDuration);
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
            if (base.Available)
            {
                if (_chosenDirection == Direction.none)
                {
                    if (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.A))
                    {
                        _chosenDirection = Direction.left;
                        StartCoroutine(ChargeDashPower());
                    }
                    else if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.E))
                    {
                        _chosenDirection = Direction.right;
                        StartCoroutine(ChargeDashPower());
                    }
                }
                else if (_chosenDirection == Direction.left)
                {
                    if (Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.A))
                    {
                        StopChargingAndExecute(_chosenDirection);
                        _chosenDirection = Direction.none;
                    }
                }
                else if (_chosenDirection == Direction.right)
                {
                    if (Input.GetKeyUp(KeyCode.Joystick1Button5) || Input.GetKeyUp(KeyCode.E))
                    {
                        StopChargingAndExecute(_chosenDirection);
                        _chosenDirection = Direction.none;
                    }
                }
            }
        }

        // ----------------------------------------------------------------- MONOBEHAVIOUR

        protected override void ValidateAttributes()
        {
            base.ValidateAttributes();
            _dashAngleInDegrees = Mathf.Clamp(_dashAngleInDegrees, 0f, 90f);
            _speed = Mathf.Max(_speed, 0f);
            _minimumDuration = Mathf.Max(_minimumDuration, 0f);
            _maximumDuration = Mathf.Max(_maximumDuration, _minimumDuration);
            _maximumChargingDuration = Mathf.Max(_maximumChargingDuration, 0f);
        }

        protected override void Initialization()
        {
            base.Initialization();
            _vehicleRigidbody = this.GetComponent<Rigidbody>();
            _chosenDirection = Direction.none;
        }
    }
}