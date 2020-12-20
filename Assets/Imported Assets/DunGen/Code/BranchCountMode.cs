using DunGen.Graph;
using System;
using System.Linq;
using UnityEngine;

using Random = System.Random;

namespace DunGen
{
	/// <summary>
	/// Used to determine how the number of branches are calculated
	/// </summary>
	public enum BranchMode
	{
		/// <summary>
		/// Branch count is calculated per-tile using the Archetype's BranchCount property
		/// </summary>
		Local,
		/// <summary>
		/// Branch count is calculated across the entire dungeon using the DungeonFlow's BranchCount property
		/// </summary>
		Global,
	}

	public static class BranchCountHelper
	{
		public static void ComputeBranchCounts(DungeonFlow dungeonFlow, Random randomStream, DungeonProxy proxyDungeon, ref int[] mainPathBranches)
		{
			switch (dungeonFlow.BranchMode)
			{
				case BranchMode.Local:
					ComputeBranchCountsLocal(randomStream, proxyDungeon, ref mainPathBranches);
					break;

				case BranchMode.Global:
					ComputeBranchCountsGlobal(dungeonFlow, randomStream, proxyDungeon, ref mainPathBranches);
					break;

				default:
					throw new NotImplementedException(string.Format("{0}.{1} is not implemented", typeof(BranchMode).Name, dungeonFlow.BranchMode));
			}
		}

		private static void ComputeBranchCountsLocal(Random randomStream, DungeonProxy proxyDungeon, ref int[] mainPathBranches)
		{
			for (int i = 0; i < mainPathBranches.Length; i++)
			{
				var tile = proxyDungeon.MainPathTiles[i];

				if (tile.Placement.Archetype == null)
					continue;

				int branchCount = tile.Placement.Archetype.BranchCount.GetRandom(randomStream);
				branchCount = Mathf.Min(branchCount, tile.UnusedDoorways.Count());

				mainPathBranches[i] = branchCount;
			}
		}

		private static void ComputeBranchCountsGlobal(DungeonFlow dungeonFlow, Random randomStream, DungeonProxy proxyDungeon, ref int[] mainPathBranches)
		{
			int globalBranchCount = dungeonFlow.BranchCount.GetRandom(randomStream);
			int totalBranchableRooms = proxyDungeon.MainPathTiles.Count(t => t.Placement.Archetype != null);
			float branchesPerTile = globalBranchCount / (float)totalBranchableRooms;

			float branchChance = branchesPerTile;
			int branchesRemaining = globalBranchCount;

			for (int i = 0; i < mainPathBranches.Length; i++)
			{
				if (branchesRemaining <= 0)
					break;

				var tile = proxyDungeon.MainPathTiles[i];

				if (tile.Placement.Archetype == null)
					continue;

				int availableDoorways = tile.UnusedDoorways.Count();
				int branchCount = Mathf.FloorToInt(branchChance);
				branchCount = Mathf.Min(branchCount, availableDoorways, tile.Placement.Archetype.BranchCount.Max);

				branchChance -= branchCount;

				if (branchCount < availableDoorways &&
					randomStream.Next() < branchChance)
				{
					branchCount++;
					branchChance = 0f;
				}

				branchChance += branchesPerTile;
				branchesRemaining -= branchCount;

				mainPathBranches[i] = branchCount;
			}
		}
	}
}
