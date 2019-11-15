using UnityEngine;
// ReSharper disable RedundantDefaultMemberInitializer

namespace AugmentedReality.Poi
{
    public abstract class BasePoi : MonoBehaviour
    {
        [SerializeField]
        public string titleTextKey = null;

        public abstract void GazeOn();

        public abstract void GazeOff();

        public abstract void Show();

        public abstract void Hide(bool immediate);

        public abstract void UpdateScale(float scale);
    }
}
