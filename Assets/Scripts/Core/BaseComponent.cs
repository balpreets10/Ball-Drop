using UnityEngine;

namespace BallDrop
{
    public class BaseComponent : BaseBehaviour
    {
        protected Context context { get; private set; }

        protected ActivationState activationState;

        public virtual void Initialize(IContext<Context> context)
        {
            this.context = context.GetContext();
        }

        protected virtual void OnContextUpdated(Context context)
        {
            this.context = context;
            OnGameStateUpdated();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected virtual void OnGameStateUpdated()
        {
        }

        public virtual void Deinitialize()
        {
        }

        public enum ActivationState
        {
            Activated,
            Deactivated
        }
    }
}