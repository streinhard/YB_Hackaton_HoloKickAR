using UnityEditor;
using UnityEngine;

namespace UI
{
    [CustomEditor(typeof(ListoryUiAnimation))]
    public class ListoryUiAnimationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            var introAnimation = (ListoryUiAnimation)target;
            if(GUILayout.Button("Intro Animation"))
            {
                introAnimation.CreateIntroAnimation();
            }
            if(GUILayout.Button("Loading Animation"))
            {
                introAnimation.CreateSpinnerAnimation();
            }
        }
    }
}