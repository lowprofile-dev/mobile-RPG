using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace DunGen.Editor
{
    public sealed class DungeonGeneratorWindow : EditorWindow
    {
        private DungeonGenerator generator = new DungeonGenerator();
        private GameObject lastDungeon;
		private bool overwriteExisting = true;


		[MenuItem("Window/DunGen/Generate Dungeon")]
        private static void OpenWindow()
        {
            EditorWindow.GetWindow<DungeonGeneratorWindow>(false, "New Dungeon", true);
        }

        private void OnGUI()
        {
            EditorUtil.DrawDungeonGenerator(generator, false);

			EditorGUILayout.Space();

			overwriteExisting = EditorGUILayout.Toggle("Overwrite Existing?", overwriteExisting);

			if (GUILayout.Button("Generate"))
				GenerateDungeon();
        }

        private void GenerateDungeon()
        {
			if (lastDungeon != null)
			{
				if (overwriteExisting)
					UnityUtil.Destroy(lastDungeon);
				else
					generator.DetachDungeon();
			}

			lastDungeon = new GameObject("Dungeon Layout");
            generator.Root = lastDungeon;

            Undo.RegisterCreatedObjectUndo(lastDungeon, "Create Procedural Dungeon");
			generator.OnGenerationStatusChanged += OnGenerationStatusChanged;
			generator.GenerateAsynchronously = false;
            generator.Generate();
        }

		private void OnGenerationStatusChanged(DungeonGenerator generator, GenerationStatus status)
		{
			if(status == GenerationStatus.Failed)
			{
				UnityUtil.Destroy(lastDungeon);
				lastDungeon = generator.Root = null;
			}
		}
	}
}