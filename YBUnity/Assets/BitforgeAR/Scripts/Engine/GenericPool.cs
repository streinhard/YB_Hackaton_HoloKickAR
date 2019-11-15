using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bitforge.Engine.Generic
{
    /// <summary>
    /// Generic pool for class instance reuse
    /// </summary>
    public class GenericPool<T> where T : class
    {
        #region Fields

        protected Func<T> _instantiateFunc = null;
        protected Action<T> _initAction = null;
        protected Action<T> _terminateAction = null;
        protected List<T> _objects = new List<T>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Contruct a new pool
        /// </summary>
        /// <param name="instantiateFunc">constructor for new object (e.g. "() => { Instantiate(GameObject); }" )</param>
        /// <param name="giveAction">called on give (e.g.  "gameObject => gameObject.SetActive(true)" )</param>
        /// <param name="takeBackAction">called on takeback (e.g.  "gameObject => gameObject.SetActive(false)" )</param>
        /// <param name="prewarmCount">object count to instantiate</param>
        public GenericPool(Func<T> instantiateFunc, Action<T> giveAction = null, Action<T> takeBackAction = null, int prewarmCount = 0)
        {
            Debug.Assert(instantiateFunc != null, "GenericPool: instantiateFunc == null");

            _instantiateFunc = instantiateFunc;
            _initAction = giveAction;
            _terminateAction = takeBackAction;

            HeatupPool(prewarmCount);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Instantiate objects until object count in pool is >= count
        /// </summary>
        public virtual void HeatupPool(int count)
        {
            while (_objects.Count < count) {
                TakeBack(_instantiateFunc());
            }
        }

        /// <summary>
        /// Get an object from pool
        /// </summary>
        public virtual T Give(bool allowCreate)
        {
            T result = null;

            if (_objects.Count > 0) {
                int index = _objects.Count - 1;
                result = _objects[index];
                _objects.RemoveAt(index);
            } else if (allowCreate) {
                result = _instantiateFunc();
            }

            if (_initAction != null)
            {
                _initAction(result);
            }

            return result;
        }

        /// <summary>
        /// Return a object to pool
        /// </summary>
        public virtual void TakeBack(T t)
        {
            if (_terminateAction != null) {
                _terminateAction(t);
            }
            _objects.Add(t);
        }

        /// <summary>
        /// Clear pool
        /// </summary>
        public virtual void Clear()
        {
            _objects.RemoveRange(0, _objects.Count);
        }

        #endregion Methods
    }
}