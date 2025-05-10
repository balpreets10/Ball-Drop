namespace BallDrop
{
    /// <summary>
    /// Can be dispatched and listened to, with up to three parameters of given types.
    /// </summary>
    public abstract class AbstractEvent
    {
        public int DispatchCount { get; private set; }

        /// <summary>
        /// Used internally by the Context to make this signal log to the IoC+ Monitor on dispatch.
        /// </summary>

        public abstract bool HasDuplicateParameterTypes();

        protected void OnDispatch()
        {
            DispatchCount++;
        }
    }

    public class MyEvent : AbstractEvent
    {
        private System.Action onTrigger;

        public MyEvent()
        {
        }

        public void Dispatch()
        {
            if (onTrigger != null)
            {
                onTrigger();
            }
            OnDispatch();
        }

        public void AddListener(System.Action handler)
        {
            onTrigger += handler;
        }

        public void RemoveListener(System.Action handler)
        {
            onTrigger -= handler;
        }

        public override bool HasDuplicateParameterTypes()
        {
            return false;
        }
    }

    /// <summary>
    /// Can be dispatched and listened to, with up to three parameters of given types.
    /// </summary>

    /// <summary>
    /// Can be dispatched and listened to, with up to three parameters of given types.
    /// </summary>
    /// <typeparam name="T">Type of the first parameter.</typeparam>
    public class MyEvent<T> : AbstractEvent
    {
        private System.Action<T> onTrigger;

        public MyEvent()
        {
        }

        public void Dispatch(T value)
        {
            if (onTrigger != null)
            {
                onTrigger(value);
            }
            OnDispatch();
        }

        public void AddListener(System.Action<T> handler)
        {
            onTrigger += handler;
        }

        public void RemoveListener(System.Action<T> handler)
        {
            onTrigger -= handler;
        }

        public override bool HasDuplicateParameterTypes()
        {
            return false;
        }
    }

    /// <summary>
    /// Can be dispatched and listened to, with up to three parameters of given types.
    /// </summary>
    /// <typeparam name="T">Type of the first parameter.</typeparam>
    /// <typeparam name="U">Type of the second parameter.</typeparam>
    public class MyEvent<T, U> : AbstractEvent
    {
        private System.Action<T, U> onTrigger;

        public MyEvent()
        {
        }

        public void Dispatch(T value, U value2)
        {
            if (onTrigger != null)
            {
                onTrigger(value, value2);
            }
            OnDispatch();
        }

        public void AddListener(System.Action<T, U> handler)
        {
            onTrigger += handler;
        }

        public void RemoveListener(System.Action<T, U> handler)
        {
            onTrigger -= handler;
        }

        public override bool HasDuplicateParameterTypes()
        {
            return typeof(T) == typeof(U);
        }
    }

    /// <summary>
    /// Can be dispatched and listened to, with up to three parameters of given types.
    /// </summary>
    /// <typeparam name="T">Type of the first parameter.</typeparam>
    /// <typeparam name="U">Type of the second parameter.</typeparam>
    /// <typeparam name="I">Type of the third parameter.</typeparam>
    public class MyEvent<T, U, I> : AbstractEvent
    {
        private System.Action<T, U, I> onTrigger;

        public MyEvent()
        {
        }

        public void Dispatch(T value, U value2, I value3)
        {
            if (onTrigger != null)
            {
                onTrigger(value, value2, value3);
            }
            OnDispatch();
        }

        public void AddListener(System.Action<T, U, I> handler)
        {
            onTrigger += handler;
        }

        public void RemoveListener(System.Action<T, U, I> handler)
        {
            onTrigger -= handler;
        }

        public override bool HasDuplicateParameterTypes()
        {
            return typeof(T) == typeof(U) ||
                   typeof(T) == typeof(I) ||
                   typeof(U) == typeof(I);
        }
    }
}