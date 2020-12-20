using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DunGen.Graph
{
	/// <summary>
	/// A graph representing the flow of a dungeon
	/// </summary>
	[Serializable]
	[CreateAssetMenu(fileName = "New Dungeon", menuName = "DunGen/Dungeon Flow", order = 700)]
	public class DungeonFlow : ScriptableObject, ISerializationCallbackReceiver
    {
		public const int FileVersion = 1;

		#region Nested Types

		[Serializable]
		public sealed class GlobalPropSettings
		{
			public int ID;
			public IntRange Count;


			public GlobalPropSettings()
			{
				ID = 0;
				Count = new IntRange(0, 1);
			}

			public GlobalPropSettings(int id, IntRange count)
			{
				ID = id;
				Count = count;
			}
		}

		#endregion

		#region Legacy Properties

		[SerializeField]
		[FormerlySerializedAs("GlobalPropGroupIDs")]
		private List<int> globalPropGroupID_obsolete = new List<int>();

		[SerializeField]
		[FormerlySerializedAs("GlobalPropRanges")]
		private List<IntRange> globalPropRanges_obsolete = new List<IntRange>();

		#endregion

		/// <summary>
		/// The minimum and maximum length of the dungeon
		/// </summary>
		public IntRange Length = new IntRange(5, 10);
		/// <summary>
		/// Determines how the number of branches from the main path is calculated
		/// </summary>
		public BranchMode BranchMode = BranchMode.Local;
		/// <summary>
		/// The number of branches to appear across the entire dungeon
		/// Only used if <see cref="BranchMode"/> is set to <see cref="BranchMode.Global"/>
		/// </summary>
		public IntRange BranchCount = new IntRange(1, 5);
		/// <summary>
		/// Information about which (and how many) global props should appear throughout the dungeon
		/// </summary>
		public List<GlobalPropSettings> GlobalProps = new List<GlobalPropSettings>();
		/// <summary>
		/// The asset that handles all of the keys that this dungeon needs to know about
		/// </summary>
		public KeyManager KeyManager = null;
		/// <summary>
		/// The percentage chance of two unconnected but overlapping doorways being connected (0-1)
		/// </summary>
		[Range(0f, 1f)]
		public float DoorwayConnectionChance = 0f;
		/// <summary>
		/// If true, only doorways belonging to tiles on the same section of the dungeon can be connected
		/// This will prevent some unexpected shortcuts from opening up through the dungeon
		/// </summary>
		public bool RestrictConnectionToSameSection = false;
		/// <summary>
		/// Simple rules for injecting special tiles into the dungeon generation process
		/// </summary>
		public List<TileInjectionRule> TileInjectionRules = new List<TileInjectionRule>();

        public List<GraphNode> Nodes = new List<GraphNode>();
        public List<GraphLine> Lines = new List<GraphLine>();

		[SerializeField]
		private int currentFileVersion;


        /// <summary>
        /// Creates the default graph
        /// </summary>
        public void Reset()
        {
			var emptyTileSet = new TileSet[0];
			var emptyArchetype = new DungeonArchetype[0];

			var builder = new DungeonFlowBuilder(this)
				.AddNode(emptyTileSet, "Start")
				.AddLine(emptyArchetype, 1.0f)
				.AddNode(emptyTileSet, "Goal");

			builder.Complete();
        }

        public GraphLine GetLineAtDepth(float normalizedDepth)
        {
            normalizedDepth = Mathf.Clamp(normalizedDepth, 0, 1);

            if (normalizedDepth == 0)
                return Lines[0];
            else if (normalizedDepth == 1)
                return Lines[Lines.Count - 1];

            foreach (var line in Lines)
                if (normalizedDepth >= line.Position && normalizedDepth < line.Position + line.Length)
                    return line;

            Debug.LogError("GetLineAtDepth was unable to find a line at depth " + normalizedDepth + ". This shouldn't happen.");
            return null;
        }

        public DungeonArchetype[] GetUsedArchetypes()
        {
            return Lines.SelectMany(x => x.DungeonArchetypes).ToArray();
        }

        public TileSet[] GetUsedTileSets()
        {
            List<TileSet> tileSets = new List<TileSet>();

            foreach (var node in Nodes)
                tileSets.AddRange(node.TileSets);

            foreach(var line in Lines)
                foreach (var archetype in line.DungeonArchetypes)
                {
                    tileSets.AddRange(archetype.TileSets);
                    tileSets.AddRange(archetype.BranchCapTileSets);
                }

            return tileSets.ToArray();
        }

		public void OnBeforeSerialize()
		{
			currentFileVersion = FileVersion;
		}

		public void OnAfterDeserialize()
		{
			// Convert to new format for Global Props
			if(currentFileVersion < 1)
			{
				for (int i = 0; i < globalPropGroupID_obsolete.Count; i++)
				{
					int id = globalPropGroupID_obsolete[i];
					var count = globalPropRanges_obsolete[i];
					GlobalProps.Add(new GlobalPropSettings(id, count));
				}

				globalPropGroupID_obsolete.Clear();
				globalPropRanges_obsolete.Clear();
			}
		}
	}
}
