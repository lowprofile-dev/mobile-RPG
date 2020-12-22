// Only available in Unity 5.6 or higher
#if !(UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5)
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace DunGen.Adapters
{
	[AddComponentMenu("DunGen/NavMesh/Unity NavMesh Adapter")]
	public class UnityNavMeshAdapter : NavMeshAdapter
	{
		#region Nested Types

		public enum RuntimeNavMeshBakeMode
		{
			/// <summary>
			/// Uses only existing baked surfaces found in the dungeon tiles, no runtime baking is performed
			/// </summary>
			PreBakedOnly,
			/// <summary>
			/// Uses existing baked surfaces in the tiles if any are found, otherwise new surfaces will be added and baked at runtime
			/// </summary>
			AddIfNoSurfaceExists,
			/// <summary>
			/// Adds new surfaces where they don't already exist. Rebakes all at runtime
			/// </summary>
			AlwaysRebake,
			/// <summary>
			/// Bakes a single surface for the entire dungeon at runtime. No links will be made
			/// </summary>
			FullDungeonBake,
		}

		[Serializable]
		public sealed class NavMeshAgentLinkInfo
		{
			public int AgentTypeID = 0;
			public int AreaTypeID = 0;
			public bool DisableLinkWhenDoorIsClosed = true;
		}

		#endregion

		public RuntimeNavMeshBakeMode BakeMode = RuntimeNavMeshBakeMode.AddIfNoSurfaceExists;
		public bool AddNavMeshLinksBetweenRooms = true;
		public List<NavMeshAgentLinkInfo> NavMeshAgentTypes = new List<NavMeshAgentLinkInfo>() { new NavMeshAgentLinkInfo() };
		public float NavMeshLinkDistanceFromDoorway = 2.5f;
		public bool AutoGenerateFullRebakeSurfaces = true;
		public List<NavMeshSurface> FullRebakeTargets = new List<NavMeshSurface>();

		private List<NavMeshSurface> addedSurfaces = new List<NavMeshSurface>();


		public override void Generate(Dungeon dungeon)
		{
			if (BakeMode == RuntimeNavMeshBakeMode.FullDungeonBake)
			{
				BakeFullDungeon(dungeon);
				return;
			}

			// Bake Surfaces
			if (BakeMode != RuntimeNavMeshBakeMode.PreBakedOnly)
			{
				foreach (var tile in dungeon.AllTiles)
				{
					// Find existing surfaces
					var existingSurfaces = tile.gameObject.GetComponentsInChildren<NavMeshSurface>();

					// Add surfaces for any agent type that is missing one
					var addedSurfaces = AddMissingSurfaces(tile, existingSurfaces);

					// Gather surfaces to bake
					IEnumerable<NavMeshSurface> surfacesToBake = addedSurfaces;

					// Append all existing surfaces if mode is set to "Always Rebake"
					if (BakeMode == RuntimeNavMeshBakeMode.AlwaysRebake)
						surfacesToBake = surfacesToBake.Concat(existingSurfaces);
					// Append only unbaked surfaces if mode is set to "Add if no Surface Exists"
					else if (BakeMode == RuntimeNavMeshBakeMode.AddIfNoSurfaceExists)
					{
						var existingUnbakedSurfaces = existingSurfaces.Where(x => x.navMeshData == null);
						surfacesToBake = surfacesToBake.Concat(existingUnbakedSurfaces);
					}


					// Bake
					foreach (var surface in surfacesToBake.Distinct())
						surface.BuildNavMesh();
				}
			}

			// Add links between rooms
			if (AddNavMeshLinksBetweenRooms)
			{
				foreach (var connection in dungeon.Connections)
					foreach (var linkInfo in NavMeshAgentTypes)
						AddNavMeshLink(connection, linkInfo);
			}

			if (OnProgress != null)
				OnProgress(new NavMeshGenerationProgress() { Description = "Done", Percentage = 1.0f });
		}

		private void BakeFullDungeon(Dungeon dungeon)
		{
			if (AutoGenerateFullRebakeSurfaces)
			{
				int settingsCount = NavMesh.GetSettingsCount();

				for (int i = 0; i < settingsCount; i++)
				{
					var settings = NavMesh.GetSettingsByIndex(i);

					var surface = dungeon.gameObject.AddComponent<NavMeshSurface>();
					surface.agentTypeID = settings.agentTypeID;
					surface.collectObjects = CollectObjects.Children;

					surface.BuildNavMesh();
				}
			}
			else
			{
				foreach (var surface in FullRebakeTargets)
					surface.BuildNavMesh();
			}

			if (OnProgress != null)
				OnProgress(new NavMeshGenerationProgress() { Description = "Done", Percentage = 1.0f });
		}

		private NavMeshSurface[] AddMissingSurfaces(Tile tile, NavMeshSurface[] existingSurfaces)
		{
			addedSurfaces.Clear();
			int settingsCount = NavMesh.GetSettingsCount();

			for (int i = 0; i < settingsCount; i++)
			{
				var settings = NavMesh.GetSettingsByIndex(i);

				// We already have a surface for this agent type
				if (existingSurfaces.Where(x => x.agentTypeID == settings.agentTypeID).Any())
					continue;

				var surface = tile.gameObject.AddComponent<NavMeshSurface>();
				surface.agentTypeID = settings.agentTypeID;
				surface.collectObjects = CollectObjects.Children;

				addedSurfaces.Add(surface);
			}

			return addedSurfaces.ToArray();
		}

		private void AddNavMeshLink(DoorwayConnection connection, NavMeshAgentLinkInfo agentLinkInfo)
		{
			var doorway = connection.A.gameObject;
			var agentSettings = NavMesh.GetSettingsByID(agentLinkInfo.AgentTypeID);

			// We need to account for the agent's radius when setting the link's width
			float linkWidth = Mathf.Max(connection.A.Socket.Size.x - (agentSettings.agentRadius * 2), 0.01f);

			// Add NavMeshLink to one of the doorways
			var link = doorway.AddComponent<NavMeshLink>();
			link.agentTypeID = agentLinkInfo.AgentTypeID;
			link.bidirectional = true;
			link.area = agentLinkInfo.AreaTypeID;
			link.startPoint = new Vector3(0, 0, -NavMeshLinkDistanceFromDoorway);
			link.endPoint = new Vector3(0, 0, NavMeshLinkDistanceFromDoorway);
			link.width = linkWidth;

			if (agentLinkInfo.DisableLinkWhenDoorIsClosed)
			{
				// If there is a door in this doorway, hookup event listeners to enable/disable the link when the door is opened/closed respectively
				GameObject doorObj = (connection.A.UsedDoorPrefabInstance != null) ? connection.A.UsedDoorPrefabInstance : (connection.B.UsedDoorPrefabInstance != null) ? connection.B.UsedDoorPrefabInstance : null;

				if (doorObj != null)
				{
					var door = doorObj.GetComponent<Door>();
					link.enabled = door.IsOpen;

					if (door != null)
						door.OnDoorStateChanged += (d, o) => link.enabled = o;
				}
			}
		}
	}
}
#endif