using Custom.PathFinding.Components;
using UnityEditor;
using UnityEngine;

namespace Editor.PathFinding.Editors
{
    [CustomEditor(typeof(ObstacleView))]
    public class ObstacleViewEditor : UnityEditor.Editor
    {
        private ObstacleView _target;
        private SerializedProperty drawRenderers;

        private void OnEnable()
        {
            _target = (ObstacleView) target;
            drawRenderers = serializedObject.FindProperty("DrawRenderers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(drawRenderers);
            foreach (var renderer in _target.GetComponentsInChildren<Renderer>())
                renderer.enabled = _target.DrawRenderers;
            serializedObject.ApplyModifiedProperties();
        }
    }
}