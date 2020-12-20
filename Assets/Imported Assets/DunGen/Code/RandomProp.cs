using UnityEngine;

namespace DunGen
{
	public abstract class RandomProp : MonoBehaviour
	{
		public virtual void Process(System.Random randomStream, Tile tile) { }
	}
}
