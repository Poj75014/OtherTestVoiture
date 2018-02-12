using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    public abstract class Ability : MonoBehaviour
    {
        private bool _available;

        protected bool Available
        {
            get { return _available; }
            set { _available = value; }
        }

        protected virtual void Initialization()
        {
            _available = true;
        }
    }
}
