using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    public abstract class CommonAbility : Ability
    {
        [SerializeField]
        private float _cooldown;
        private float _remainingCooldown;

        public float RemainingCooldown
        {
            get { return _remainingCooldown; }
        }

        public float Cooldown
        {
            get { return _cooldown; }
        }

        protected IEnumerator LaunchCooldown()
        {
            base.Available = false;
            _remainingCooldown = _cooldown;

            while (_remainingCooldown > 0)
            {
                yield return null;
                _remainingCooldown -= Time.deltaTime;
            }

            base.Available = true;
        }

        public void RefreshCooldown()
        {
            base.Available = true;
            StopCoroutine(LaunchCooldown());
        }

        protected virtual void ValidateAttributes()
        {
            _cooldown = Mathf.Max(_cooldown, 0f);
        }
    }
}