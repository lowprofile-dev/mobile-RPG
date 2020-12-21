using UnityEngine;
using UnityEditor;

namespace DunGen.Editor
{
	[CustomEditor(typeof(RandomPrefab))]
    public class RandomPrefabInspector : UnityEditor.Editor
    {
		#region Labels

		private static class Label
		{
			public static readonly GUIContent ZeroPosition = new GUIContent("Zero Position", "Snaps the spawned prop to this GameObject's position. Otherwise, the prefab's position will be used as an offset.");
			public static readonly GUIContent ZeroRotation =new GUIContent("Zero Rotation", "Snaps the spawned prop to this GameObject's rotation. Otherwise, the prefab's rotation will be used as an offset.");
			public static readonly GUIContent Props = new GUIContent("Prefab", "Snaps the spawned prop to this GameObject's rotation. Otherwise, the prefab's rotation will be used as an offset.");
		}

		#endregion

		private SerializedProperty zeroPosition;
		private SerializedProperty zeroRotation;
		private SerializedProperty props;


        private void OnEnable()
        {
			zeroPosition = serializedObject.FindProperty("ZeroPosition");
			zeroRotation = serializedObject.FindProperty("ZeroRotation");
			props = serializedObject.FindProperty("Props");
        }

        public override void OnInspectorGUI()
        {
			serializedObject.Update();

			EditorGUILayout.PropertyField(zeroPosition, Label.ZeroPosition);
			EditorGUILayout.PropertyField(zeroRotation, Label.ZeroRotation);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(props, Label.Props);

			serializedObject.ApplyModifiedProperties();
        }
    }
}

