using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DunGen
{
	public sealed class InjectedTile
	{
		public TileSet TileSet;
		public float NormalizedPathDepth;
		public float NormalizedBranchDepth;
		public bool IsOnMainPath;
		public bool IsRequired;


		public InjectedTile(TileSet tileSet, bool isOnMainPath, float normalizedPathDepth, float normalizedBranchDepth, bool isRequired = false)
		{
			TileSet = tileSet;
			IsOnMainPath = isOnMainPath;
			NormalizedPathDepth = normalizedPathDepth;
			NormalizedBranchDepth = normalizedBranchDepth;
			IsRequired = isRequired;
		}

		public bool ShouldInjectTileAtPoint(bool isOnMainPath, float pathDepth, float branchDepth)
		{
			if (IsOnMainPath != isOnMainPath)
				return false;

			if (NormalizedPathDepth > pathDepth)
				return false;
			else if (isOnMainPath)
				return true;

			return NormalizedBranchDepth <= branchDepth;
		}
	}
}
