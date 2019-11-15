using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CameraHelper
{
    [RequireComponent(typeof(Camera))]
    public class ClipPlaneOptimizer : MonoBehaviour
    {
        private const float SMALL_VALUE_ROUND_FACTOR = 10;
        private const float BIG_VALUE_ROUND_FACTOR = 5;

        [Range(1, 60)]
        public byte everyXFrame = 1;

        protected readonly List<Renderer> Renderers = new List<Renderer>();

        private Camera _camera;
        private readonly Plane[] _planes = new Plane[6];
        private float _awakeNearClipPlane;
        private float _awakeFarClipPlane;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _awakeNearClipPlane = _camera.nearClipPlane;
            _awakeFarClipPlane = _camera.farClipPlane;
        }

        private void OnEnable()
        {
            UpdateRendererList();
        }

        private void OnDisable()
        {
            _camera.nearClipPlane = _awakeNearClipPlane;
            _camera.farClipPlane = _awakeFarClipPlane;
        }

        private void Update()
        {
            var frameIndex = Time.frameCount;
            if (everyXFrame <= 1 || Time.frameCount % frameIndex == 0) {
                if (Renderers.Count <= 0) { UpdateRendererList(); }

                SetClipPlaneBase();
            }
        }

        private void SetClipPlaneBase()
        {
            _camera.nearClipPlane = _awakeNearClipPlane;
            _camera.farClipPlane = _awakeFarClipPlane;

            if (Renderers.Count > 0) {
                // get frustrum planes
                GeometryUtility.CalculateFrustumPlanes(_camera, _planes);

                var minSqrMagnitude = float.MaxValue;
                var maxSqrMagnitude = float.MinValue;
                var cameraTransform = _camera.transform;
                var cameraPosition = cameraTransform.position;
                var cameraForward = cameraTransform.forward;
                var foundAtLeastOne = false;

                // find min and max
                foreach (var r in Renderers) {
                    if (r.isVisible) {
                        var bounds = r.bounds;

                        // if inside an object return directly with standard values
                        /*if (bounds.Contains(cameraPosition)) {
                            _camera.nearClipPlane = _awakeNearClipPlane;
                            _camera.farClipPlane = _awakeFarClipPlane;
                            return;
                        }*/

                        if (GeometryUtility.TestPlanesAABB(_planes, bounds)) {
                            var oppositeCameraPosition = cameraPosition + _awakeFarClipPlane * cameraForward;
                            var minPoint = bounds.ClosestPoint(cameraPosition);
                            var maxPoint = bounds.ClosestPoint(oppositeCameraPosition);

                            var newMinSqrDistance =
                                Vector3.Project(minPoint - cameraPosition, cameraForward).sqrMagnitude;
                            minSqrMagnitude = Mathf.Min(minSqrMagnitude, newMinSqrDistance);

                            var newMaxSqrDistance =
                                Vector3.Project(maxPoint - cameraPosition, cameraForward).sqrMagnitude;
                            maxSqrMagnitude = Mathf.Max(maxSqrMagnitude, newMaxSqrDistance);

                            foundAtLeastOne = true;
                        }
                    }
                }

                // if no renderer was insight exit immediate
                if (!foundAtLeastOne) { return; }

                // make it a little big bigger or smaller
                minSqrMagnitude *= 0.99f;
                maxSqrMagnitude /= 0.99f;

                // get rid of sqr
                minSqrMagnitude = Mathf.Sqrt(minSqrMagnitude);
                maxSqrMagnitude = Mathf.Sqrt(maxSqrMagnitude);

                // stepify it to get ride of border noise
                minSqrMagnitude = minSqrMagnitude < 1 ?
                    Mathf.Floor(minSqrMagnitude * SMALL_VALUE_ROUND_FACTOR) / SMALL_VALUE_ROUND_FACTOR :
                    Mathf.Floor(minSqrMagnitude * BIG_VALUE_ROUND_FACTOR) / BIG_VALUE_ROUND_FACTOR;
                maxSqrMagnitude = maxSqrMagnitude < 1 ?
                    Mathf.Ceil(maxSqrMagnitude * SMALL_VALUE_ROUND_FACTOR) / SMALL_VALUE_ROUND_FACTOR :
                    Mathf.Ceil(maxSqrMagnitude * BIG_VALUE_ROUND_FACTOR) / BIG_VALUE_ROUND_FACTOR;

                // limit values by original values
                minSqrMagnitude = Mathf.Max(minSqrMagnitude, _awakeNearClipPlane);
                maxSqrMagnitude = Mathf.Min(maxSqrMagnitude, _awakeFarClipPlane);

                // max should always be bigger than min
                maxSqrMagnitude = Mathf.Max(minSqrMagnitude * 1.3f, maxSqrMagnitude);

                _camera.nearClipPlane = minSqrMagnitude;
                _camera.farClipPlane = maxSqrMagnitude;
            }
        }

        [ContextMenu("Print Renderers")]
        private void PrintRendererList()
        {
            var builder = new StringBuilder("ClipRenderers:\n");
            foreach (var r in Renderers) { builder.AppendLine(r.name); }

            Debug.Log(builder.ToString());
        }

        public void TryToAdd(Component c)
        {
            if (c != null) {
                var rs = c.GetComponentsInChildren<Renderer>(true);
                if (rs != null && rs.Length > 0) {
                    foreach (var r in rs) { Add(r); }
                }
            }
        }

        public void TryToRemove(Component c)
        {
            if (c != null) {
                var rs = c.GetComponentsInChildren<Renderer>(true);
                if (rs != null && rs.Length > 0) {
                    foreach (var r in rs) { Remove(r); }
                }
            }
        }

        public void Add(Renderer r)
        {
            if (r != null && !Renderers.Contains(r)) { Renderers.Add(r); }
        }

        public void Remove(Renderer r)
        {
            if (r != null) { Renderers.Remove(r); }
        }

        [ContextMenu("Update Renderer List")]
        protected virtual void UpdateRendererList()
        {
            Renderers.Clear();
            var allGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var go in allGameObjects) { Renderers.AddRange(go.GetComponentsInChildren<Renderer>(true)); }
        }
    }
}
