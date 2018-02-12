using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    public class Jump : CommonAbility
    {
        [SerializeField]
        private float _initialSpeed;

        private Rigidbody _vehicleRigidbody;




        private void Execute()
        {
            Vector2 angle = MathsTools.DegreesToVector2(90f);
            Vector3 originalSpeed = _vehicleRigidbody.velocity;
            _vehicleRigidbody.velocity = transform.TransformVector(new Vector3(0f, -angle.y, angle.x) * _initialSpeed)
                                            + originalSpeed;
            StartCoroutine(base.LaunchCooldown());
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
            if (base.Available && Input.GetKeyDown(KeyCode.Space))
                this.Execute();
        }

        // ----------------------------------------------------------------- MONOBEHAVIOUR

        protected override void ValidateAttributes()
        {
            base.ValidateAttributes();
            _initialSpeed = Mathf.Max(_initialSpeed, 0f);
        }

        protected override void Initialization()
        {
            base.Initialization();
            _vehicleRigidbody = this.GetComponent<Rigidbody>();
        }

    }
}