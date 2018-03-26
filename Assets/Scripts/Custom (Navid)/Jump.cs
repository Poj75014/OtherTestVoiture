using System.Collections;
using UnityEngine;

namespace RVP
{
    public class Jump : MonoBehaviour
    {
        public float speed = 15f;
        public float dashAngleInDegrees = 90f;
        public float cooldown = 3f;
        public Suspension[] suspensions;
        public float newSpringDampening = 5f;

        private bool available;
        private Rigidbody vehicleRigidbody;

        private void ValidateAttributes()
        {
            this.dashAngleInDegrees = Mathf.Clamp(this.dashAngleInDegrees, 0f, 90f);
            this.speed = Mathf.Max(this.speed, 0f);
            this.cooldown = Mathf.Max(this.cooldown, 0f);
        }

        // Use this for initialization
        void Start()
        {
            this.Initialization();
            this.vehicleRigidbody = this.GetComponent<Rigidbody>();
        }


        void FixedUpdate()
        {
            if ((Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.LeftControl)) && this.available)
                this.Execute();
            Debug.Log(this.vehicleRigidbody.position);
        }

        private void Initialization()
        {
            this.available = true;
        }

        private void Execute()
        {
            Vector2 angle = MathsTools.DegreesToVector2(this.dashAngleInDegrees);
            Vector3 oldSpeed = this.vehicleRigidbody.velocity;
            this.vehicleRigidbody.velocity = transform.TransformVector(new Vector3(0f, -angle.y, angle.x) * this.speed) + oldSpeed;
            //foreach (Suspension suspension in suspensions)
            //{
            //    suspension.GetComponent<Suspension>().springDampening = newSpringDampening;
            //}
            //StartCoroutine(Delay());
            StartCoroutine(Cooldown());
        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (Suspension suspension in suspensions)
            {
                //suspension.GetComponent<Suspension>().springDampening = 1;
            }
        }

        private IEnumerator Delay()
        {
            this.available = false;
            yield return new WaitForSeconds(1);
        }

        private IEnumerator Cooldown()
        {
            this.available = false;
            yield return new WaitForSeconds(this.cooldown);
            this.available = true;
        }

    }
}