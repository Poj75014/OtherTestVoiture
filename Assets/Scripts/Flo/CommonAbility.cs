using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    public abstract class CommonAbility : Ability
    {
        [SerializeField]
        private float _cooldown;

        protected float Cooldown
        {
            get { return _cooldown; }
            set { _cooldown = value; }
        }

        public IEnumerator LaunchCooldown()
        {
            Available = false;
            yield return new WaitForSeconds(Cooldown);
            Available = true;
        }

        public void RefreshCooldown()
        {
            this.Available = true;
            StopCoroutine(LaunchCooldown());
        }

        protected virtual void ValidateAttributes()
        {
            _cooldown = Mathf.Max(_cooldown, 0f);
        }
    }
}