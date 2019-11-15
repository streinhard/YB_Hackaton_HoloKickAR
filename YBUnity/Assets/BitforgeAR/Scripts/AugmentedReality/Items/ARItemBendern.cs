using DG.Tweening;
using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable InconsistentNaming

namespace AugmentedReality.Items
{
    public class ARItemBendern : ARItemDefault
    {
        [SerializeField]
        private MeshRenderer ground = null;

        [SerializeField]
        private Material[] groundMaterials = null;

        [SerializeField]
        private Material groundMaterialNeutral = null;

        [SerializeField]
        private Transform[] buildingAnchors = null;

        [SerializeField]
        private string[] buildingTitleKeys = null;

        [SerializeField]
        private string[] buildingTextKeys = null;

        public int BuildingCount => Mathf.Min(buildingAnchors.Length, groundMaterials.Length, buildingTitleKeys.Length, buildingTextKeys.Length);

        public string CurrentBuildingTitleKey => buildingTitleKeys[_currentBuildingIndex];
        public string CurrentBuildingTextKey => buildingTextKeys[_currentBuildingIndex];

        private int _currentBuildingIndex = -1;
        private Tween _currentTween;

        protected override void Start()
        {
            SetBuilding(0, true);
            base.Start();
        }

        public override void InitAndHide()
        {
            // scale all to zero without the selected one
            for (var i = 0; i < buildingAnchors.Length; i++) {
                if (i != _currentBuildingIndex) {
                    var buildingAnchor = buildingAnchors[i];
                    buildingAnchor.localScale = Vector3.zero;
                }
            }

            base.InitAndHide();
        }

        public void SetBuilding(int buildingIndex, bool immediate = false)
        {
            buildingIndex = Mathf.Clamp(buildingIndex, 0, BuildingCount);

            // kill animations
            _currentTween?.Kill();

            if (immediate) {
                // remove old building
                if (_currentBuildingIndex >= 0) { buildingAnchors[_currentBuildingIndex].localScale = Vector3.zero; }

                // replace with new one
                _currentBuildingIndex = buildingIndex;
                ground.sharedMaterial = groundMaterials[_currentBuildingIndex];
                buildingAnchors[_currentBuildingIndex].localScale = Vector3.one;
            }
            else {
                var sequence = DOTween.Sequence();

                // remove old building
                if (_currentBuildingIndex >= 0) {
                    sequence.Insert(0, buildingAnchors[_currentBuildingIndex].DOScale(Vector3.zero, 0.2f));
                    sequence.InsertCallback(
                        0f,
                        () => { ground.sharedMaterial = groundMaterialNeutral; }
                    );
                }

                // replace with new one
                _currentBuildingIndex = buildingIndex;
                sequence.Insert(0.1f, buildingAnchors[_currentBuildingIndex].DOScale(Vector3.one, 0.2f));
                sequence.InsertCallback(
                    0.25f,
                    () => { ground.sharedMaterial = groundMaterials[_currentBuildingIndex]; }
                );

                _currentTween = sequence;
            }
        }
    }
}
