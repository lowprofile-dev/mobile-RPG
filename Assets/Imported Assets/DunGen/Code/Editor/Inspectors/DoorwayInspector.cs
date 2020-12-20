using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace DunGen.Editor
{
	[CustomEditor(typeof(Doorway))]
	[CanEditMultipleObjects]
	public class DoorwayInspector : UnityEditor.Editor
	{
		#region Constants

		private static readonly GUIContent socketGroupLabel = new GUIContent("Socket", "Determines if two doorways can connect. By default, only doorways with matching socket groups can be connected to one another");
		private static readonly GUIContent hideConditionalObjectsLabel = new GUIContent("Hide Conditional Objects?", "If checked, any in-scene door or blocked objects will be hidden for the purpose of reducing clutter. Has no effect on the runtime results");
		private static readonly GUIContent connectorSceneObjectsLabel = new GUIContent("Scene Objects", "In-scene objects to be KEPT when the doorway is in use (connected). Objects are kept on both sides of the doorway");
		private static readonly GUIContent blockerSceneObjectsLabel = new GUIContent("Scene Objects", "In-scene objects to be REMOVED when the doorway is in use (connected)");
		private static readonly GUIContent priorityLabel = new GUIContent("Priority", "When two doorways are connected, the one with the higher priority will have their door prefab used");
		private static readonly GUIContent doorPrefabLabel = new GUIContent("Random Prefab Weights", "When this doorway is in use (connected), a single prefab will be spawned from this list (and the connected doorway) at random");
		private static readonly GUIContent blockerPrefabLabel = new GUIContent("Random Prefab Weights", "When this doorway is NOT in use (unconnected), a single prefab will be spawned from this list (and the connected doorway) at random");
		private static readonly GUIContent avoidRotationLabel = new GUIContent("Avoid Rotation?", "If checked, the placed prefab will NOT be oriented to match the doorway");
		private static readonly GUIContent connectorsLabel = new GUIContent("Connectors", "In-scene objects and prefabs used when the doorway is in use (connected)");
		private static readonly GUIContent blockersLabel = new GUIContent("Blockers", "In-scene objects and prefabs used when the doorway is not in use (not connected)");

		#endregion

		private SerializedProperty socketProp;
		private SerializedProperty hideConditionalObjectsProp;
		private SerializedProperty priorityProp;
		private SerializedProperty avoidDoorPrefabRotationProp;
		private SerializedProperty avoidBlockerPrefabRotationProp;
		private ReorderableList connectorSceneObjectsList;
		private ReorderableList blockerSceneObjectsList;
		private ReorderableList connectorPrefabsList;
		private ReorderableList blockerPrefabsList;


		private void OnEnable()
		{
			socketProp = serializedObject.FindProperty("socket");
			hideConditionalObjectsProp = serializedObject.FindProperty("hideConditionalObjects");
			priorityProp = serializedObject.FindProperty("DoorPrefabPriority");
			avoidDoorPrefabRotationProp = serializedObject.FindProperty("AvoidRotatingDoorPrefab");
			avoidBlockerPrefabRotationProp = serializedObject.FindProperty("AvoidRotatingBlockerPrefab");


			connectorSceneObjectsList = new ReorderableList(serializedObject, serializedObject.FindProperty("ConnectorSceneObjects"), true, true, true, true);
			connectorSceneObjectsList.drawElementCallback = (rect, index, isActive, isFocused) => DrawGameObject(connectorSceneObjectsList, rect, index, true);
			connectorSceneObjectsList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, connectorSceneObjectsLabel);

			blockerSceneObjectsList = new ReorderableList(serializedObject, serializedObject.FindProperty("BlockerSceneObjects"), true, true, true, true);
			blockerSceneObjectsList.drawElementCallback = (rect, index, isActive, isFocused) => DrawGameObject(blockerSceneObjectsList, rect, index, true);
			blockerSceneObjectsList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, blockerSceneObjectsLabel);

			connectorPrefabsList = new ReorderableList(serializedObject, serializedObject.FindProperty("ConnectorPrefabWeights"), true, true, true, true);
			connectorPrefabsList.drawElementCallback = (rect, index, isActive, isFocused) => DrawGameObjectWeight(connectorPrefabsList, rect, index, false);
			connectorPrefabsList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, doorPrefabLabel);
			connectorPrefabsList.onAddCallback = OnAddGameObjectChance;
			
			blockerPrefabsList = new ReorderableList(serializedObject, serializedObject.FindProperty("BlockerPrefabWeights"), true, true, true, true);
			blockerPrefabsList.drawElementCallback = (rect, index, isActive, isFocused) => DrawGameObjectWeight(blockerPrefabsList, rect, index, false);
			blockerPrefabsList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, blockerPrefabLabel);
			blockerPrefabsList.onAddCallback = OnAddGameObjectChance;
		}

		private void OnAddGameObjectChance(ReorderableList list)
		{
			list.serializedProperty.arraySize++;

			int newIndex = list.serializedProperty.arraySize - 1;
			var newElement = list.serializedProperty.GetArrayElementAtIndex(newIndex);

			newElement.FindPropertyRelative("Weight").floatValue = 1f;
		}

		private void DrawGameObject(ReorderableList list, Rect rect, int index, bool requireSceneObject)
		{
			rect = new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.BeginChangeCheck();

			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			var newObject = EditorGUI.ObjectField(rect, element.objectReferenceValue, typeof(GameObject), requireSceneObject);
			bool isValidEntry = true;

			if (newObject != null)
			{
				bool isAsset = EditorUtility.IsPersistent(newObject);
				isValidEntry = isAsset != requireSceneObject;
			}

			if (EditorGUI.EndChangeCheck() && isValidEntry)
				element.objectReferenceValue = newObject;
		}

		private void DrawGameObjectWeight(ReorderableList list, Rect rect, int index, bool requireSceneObject)
		{
			rect = new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight);

			const float weightWidth = 100f;

			Rect gameObjectRect = rect;
			gameObjectRect.width -= weightWidth;

			Rect weightRect = rect;
			weightRect.width = weightWidth;
			weightRect.x += gameObjectRect.width;

			EditorGUI.BeginChangeCheck();

			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			var gameObjectProperty = element.FindPropertyRelative("GameObject");
			var weightProperty = element.FindPropertyRelative("Weight");

			var newObject = EditorGUI.ObjectField(gameObjectRect, gameObjectProperty.objectReferenceValue, typeof(GameObject), requireSceneObject);
			bool isValidEntry = true;

			if (newObject != null)
			{
				bool isAsset = EditorUtility.IsPersistent(newObject);
				isValidEntry = isAsset != requireSceneObject;
			}

			if (EditorGUI.EndChangeCheck() && isValidEntry)
				gameObjectProperty.objectReferenceValue = newObject;

			EditorGUI.PropertyField(weightRect, weightProperty, GUIContent.none);
		}

		public override void OnInspectorGUI()
		{
			var doorways = targets.Cast<Doorway>();
			serializedObject.Update();

			if (socketProp.objectReferenceValue == null)
				socketProp.objectReferenceValue = DunGenSettings.Instance.DefaultSocket;

			EditorGUILayout.PropertyField(socketProp, socketGroupLabel);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(hideConditionalObjectsProp, hideConditionalObjectsLabel);
			if (EditorGUI.EndChangeCheck())
			{
				foreach(var d in doorways)
					d.HideConditionalObjects = hideConditionalObjectsProp.boolValue;
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUI.indentLevel++;

			// Connectors
			EditorGUILayout.BeginVertical("box");

			priorityProp.isExpanded = EditorGUILayout.Foldout(priorityProp.isExpanded, connectorsLabel, true);
			if (priorityProp.isExpanded)
			{
				EditorGUILayout.PropertyField(priorityProp, priorityLabel);
				EditorGUILayout.PropertyField(avoidDoorPrefabRotationProp, avoidRotationLabel);

				EditorGUILayout.Space();

				connectorPrefabsList.DoLayoutList();

				EditorGUILayout.Space();

				connectorSceneObjectsList.DoLayoutList();
			}
			EditorGUILayout.EndVertical();

			// Blockers
			EditorGUILayout.BeginVertical("box");

			avoidBlockerPrefabRotationProp.isExpanded = EditorGUILayout.Foldout(avoidBlockerPrefabRotationProp.isExpanded, blockersLabel, true);
			if (avoidBlockerPrefabRotationProp.isExpanded)
			{
				EditorGUILayout.PropertyField(avoidBlockerPrefabRotationProp, avoidRotationLabel);

				EditorGUILayout.Space();

				blockerPrefabsList.DoLayoutList();

				EditorGUILayout.Space();

				blockerSceneObjectsList.DoLayoutList();
			}
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;

			serializedObject.ApplyModifiedProperties();
		}
	}
}