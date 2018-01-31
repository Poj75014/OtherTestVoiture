using UnityEngine;

namespace RVP
{
    public class Boost : MonoBehaviour {

        private Rigidbody carRigidbody;
        private VehicleParent vp;
        public float boostForce = 50000;
        public float downforceDuringBoost = 200;
        public ParticleSystem[] boostParticles;

        // Use this for initialization
        void Start() {
            vp = GetComponent<VehicleParent>();
            carRigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update() {
            bool boosting = vp.boostButton;
            if (boosting)
            {
                carRigidbody.AddForce(transform.forward * boostForce);
                carRigidbody.AddForce(transform.up * -downforceDuringBoost);

            }
            //Play boost particles
            if (boostParticles.Length > 0)
            {
                foreach (ParticleSystem curBoost in boostParticles)
                {
                    if (boosting && curBoost.isStopped)
                    {
                        curBoost.Play();
                    }
                    else if (!boosting && curBoost.isPlaying)
                    {
                        curBoost.Stop();
                    }
                }
            }
        }
    }
}
