﻿using System;
using System.Collections.Generic;

namespace game.core.Common
{
    public class Whistle<T, T2> : Whistle, IWhistle<T, T2>
    {
        public void Dispatch(T context, T2 context2)
        {
            foreach (var callback in _callbacks) {
                if (callback is Action<T, T2> action) {
                    action(context, context2);
                }
                else {
                    callback.DynamicInvoke(context, context2);
                }
            }
        }

        public void Add(Action<T, T2> action) => base.Add(action);

        public void Remove(Action<T, T2> action) => base.Remove(action);
    }
    
    public class Whistle<T> : Whistle, IWhistle<T>
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
    public class Whistle : IWhistle
    {
        protected HashSet<Delegate> _callbacks;

        public Whistle() {
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

    public interface IWhistle<out T, out T2> : IWhistle
    {
        void Add(Action<T, T2> action);
        void Remove(Action<T, T2> action);
    }
    
    public interface IWhistle<out T> : IWhistle
    {
        void Add(Action<T> action);
        void Remove(Action<T> action);
    }

    public interface IWhistle
    {
        void Add(Delegate action);
        void Remove(Delegate action);
    }
}