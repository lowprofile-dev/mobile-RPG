using DunGen.Graph;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DunGen
{
    public struct ProxyDoorwayConnection
    {
        public DoorwayProxy A { get; private set; }
        public DoorwayProxy B { get; private set; }


        public ProxyDoorwayConnection(DoorwayProxy a, DoorwayProxy b)
        {
            A = a;
            B = b;
        }
    }

	public sealed class DungeonProxy
	{
		public List<TileProxy> AllTiles = new List<TileProxy>();
		public List<TileProxy> MainPathTiles = new List<TileProxy>();
		public List<TileProxy> BranchPathTiles = new List<TileProxy>();
        public List<ProxyDoorwayConnection> Connections = new List<ProxyDoorwayConnection>();

		private Transform visualsRoot;
		private Dictionary<TileProxy, GameObject> tileVisuals = new Dictionary<TileProxy, GameObject>();


		public DungeonProxy(Transform debugVisualsRoot = null)
		{
			visualsRoot = debugVisualsRoot;
		}

		public void ClearDebugVisuals()
		{
			var instances = tileVisuals.Values.ToArray();

			foreach (var instance in instances)
				GameObject.DestroyImmediate(instance);

			tileVisuals.Clear();
		}

        public void MakeConnection(DoorwayProxy a, DoorwayProxy b)
        {
            Debug.Assert(a != null && b != null);
            Debug.Assert(a != b);
            Debug.Assert(!a.Used && !b.Used);

            DoorwayProxy.Connect(a, b);
            var conn = new ProxyDoorwayConnection(a, b);
            Connections.Add(conn);
        }

		public void RemoveLastConnection()
		{
            Debug.Assert(Connections.Any(), "No connections to remove");
            var conn = Connections.Last();

            conn.A.Disconnect();
            Connections.Remove(conn);
        }

        internal void AddTile(TileProxy tile)
        {
            AllTiles.Add(tile);

            if (tile.Placement.IsOnMainPath)
                MainPathTiles.Add(tile);
            else
                BranchPathTiles.Add(tile);

			if(visualsRoot != null)
			{
				var tileObj = GameObject.Instantiate(tile.Prefab, visualsRoot);
				tileObj.transform.localPosition = tile.Placement.Position;
				tileObj.transform.localRotation = tile.Placement.Rotation;

				tileVisuals[tile] = tileObj;
			}
        }

        internal void RemoveTile(TileProxy tile)
        {
            AllTiles.Remove(tile);

            if (tile.Placement.IsOnMainPath)
                MainPathTiles.Remove(tile);
            else
                BranchPathTiles.Remove(tile);

			GameObject tileInstance;
			if(tileVisuals.TryGetValue(tile, out tileInstance))
			{
				GameObject.DestroyImmediate(tileInstance);
				tileVisuals.Remove(tile);
			}	
        }

		internal void ConnectOverlappingDoorways(float globalChance, DungeonFlow dungeonFlow, System.Random randomStream)
		{
			const float epsilon = 0.00001f;
			var doorways = AllTiles.SelectMany(t => t.UnusedDoorways);

			foreach (var a in doorways)
			{
				foreach (var b in doorways)
				{
					// Don't try to connect doorways that are already connected to another
					if (a.Used || b.Used)
						continue;

					// Don't try to connect doorways to themselves
					if (a == b || a.TileProxy == b.TileProxy)
						continue;

					// These doors cannot be connected due to their sockets
					if (!DoorwaySocket.CanSocketsConnect(a.Socket, b.Socket))
						continue;

					float distanceSqrd = (a.Position - b.Position).sqrMagnitude;

					// The doorways are too far apart
					if (distanceSqrd >= epsilon)
						continue;

					if (dungeonFlow.RestrictConnectionToSameSection)
					{
						bool tilesAreOnSameLineSegment = a.TileProxy.Placement.GraphLine == b.TileProxy.Placement.GraphLine;

						// The tiles are not on a line segment
						if (a.TileProxy.Placement.GraphLine == null)
							tilesAreOnSameLineSegment = false;

						if (!tilesAreOnSameLineSegment)
							continue;
					}

					float chance = globalChance;

					// Allow tiles to override the global connection chance
					// If both tiles want to override the connection chance, use the lowest value
					if (a.TileProxy.PrefabTile.OverrideConnectionChance && b.TileProxy.PrefabTile.OverrideConnectionChance)
						chance = Mathf.Min(a.TileProxy.PrefabTile.ConnectionChance, b.TileProxy.PrefabTile.ConnectionChance);
					else if (a.TileProxy.PrefabTile.OverrideConnectionChance)
						chance = a.TileProxy.PrefabTile.ConnectionChance;
					else if (b.TileProxy.PrefabTile.OverrideConnectionChance)
						chance = b.TileProxy.PrefabTile.ConnectionChance;

					// There is no chance to connect these doorways
					if (chance <= 0f)
						continue;

					if (randomStream.NextDouble() < chance)
						MakeConnection(a, b);
				}
			}
		}
	}
}
