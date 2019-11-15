using AugmentedReality.Items;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace CameraHelper
{
    public class ARItemClipPlaneOptimizer : ClipPlaneOptimizer
    {
        protected override void UpdateRendererList()
        {
            Renderers.Clear();

            // find all renderers inside
            var arItem = FindObjectOfType<ARItem>();
            Renderers.AddRange(arItem.GetComponentsInChildren<Renderer>(true));
            
            // filter unused line renderer
            for (var i = Renderers.Count - 1; i >= 0; i--) {
                var r = Renderers[i];
                if (r is LineRenderer) {
                    Renderers.RemoveAt(i);
                }
            }
        }
    }
}
