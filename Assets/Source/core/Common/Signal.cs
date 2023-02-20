using System;
using System.Collections.Generic;

namespace game.core.Common
{
    public class Signal<T> : Signal, ISignal<T>
    {
        public void Dispatch(T context)
        {
            foreach (var callback in _callbacks) {
                if (callback is Action<T> action) {
                    action(context);
                }
                else {
                    callback.DynamicInvoke(context);
                }
            }
        }

        public void Add(Action<T> action) => base.Add(action);

        public void Remove(Action<T> action) => base.Remove(action);
    }
    public class Signal : ISignal
    {
        protected HashSet<Delegate> _callbacks;

        public Signal() {
            _callbacks = new HashSet<Delegate>();
        }

        public void Add(Delegate action) {
            _callbacks.Add(action);
        }

        public void Remove(Delegate action) {
            _callbacks.Remove(action);
        }

        public virtual void Dispatch() {
            foreach (var callback in _callbacks) {
                if (callback is Action action) {
                    action();
                }
                else {
                    callback.DynamicInvoke();
                }
            }
        }
        
        public void Clear() {
            _callbacks.Clear();
        }
    }

    public interface ISignal<T> : ISignal
    {
        void Add(Action<T> action);
        void Remove(Action<T> action);
    }

    public interface ISignal
    {
        void Add(Delegate action);
        void Remove(Delegate action);
    }
}