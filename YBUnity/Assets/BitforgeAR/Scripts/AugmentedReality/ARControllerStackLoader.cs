using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantDefaultMemberInitializer

namespace AugmentedReality
{
    public class ARControllerStackLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject debugStackPrefab = null;

        [SerializeField]
        private GameObject arFoundationStackPrefab = null;

        [SerializeField]
        private GameObject eventSystemPrefab = null;

        private GameObject _controllerStack;
        private static Camera _arCamera;
        
        private void Awake()
        {
            // instantiate event system if there is non in scene
            // typically in debug mode started directly in ar scene
            var eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null) { Instantiate(eventSystemPrefab); }

            // unload an old stack if there is one
            UnloadControllerStack();
            _controllerStack = Instantiate(Application.isEditor ? debugStackPrefab : arFoundationStackPrefab);

            _arCamera = _controllerStack.GetComponentInChildren<IARSessionController>().ARCamera;
        }

        private void OnDisable()
        {
            _arCamera = null;
            UnloadControllerStack();
        }

        private void UnloadControllerStack()
        {
            if (_controllerStack != null) {
                _controllerStack.SetActive(false);
                Destroy(_controllerStack);
                _controllerStack = null;
            }
        }

        public static Camera GetArCamera()
        {
            if (_arCamera == null) {
                var arSessionController = (IARSessionController) FindObjectOfType<ARFoundationSessionController>() ??
                    FindObjectOfType<ARDebugSessionController>();
                _arCamera = arSessionController.ARCamera;
            }

            return _arCamera;
        }
    }
}
