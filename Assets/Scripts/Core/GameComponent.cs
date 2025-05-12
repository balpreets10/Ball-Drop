using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BallDrop
{
    public class GameComponent : BaseComponent
    {
        public ComponentEvents componentEvents = new ComponentEvents();
        public GameObject componentObject;

        public override void Activate()
        {
            base.Activate();
            if (componentObject && !componentObject.activeInHierarchy)
            {
                activationState = ActivationState.Activated;
                componentObject.SetActive(true);
                componentEvents.OnComponentActivated.Invoke();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            if (componentObject)
            {
                activationState = ActivationState.Deactivated;
                componentObject.SetActive(false);
                componentEvents.OnComponentDeactivated.Invoke();
            }
        }

        public bool IsActiveInHeirarchy()
        {
            if (componentObject)
                return componentObject.activeInHierarchy;
            else
                return gameObject.activeInHierarchy;
        }

        [Serializable]
        public class ComponentEvents
        {
            public UnityEvent OnComponentActivated = new UnityEvent();
            public UnityEvent OnComponentDeactivated = new UnityEvent();
        }
    }
}