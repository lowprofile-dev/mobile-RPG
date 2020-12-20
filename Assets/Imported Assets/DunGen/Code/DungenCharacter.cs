using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DunGen
{
	public delegate void DungenCharacterDelegate(DungenCharacter character);
    public delegate void CharacterTileChangedEvent(DungenCharacter character, Tile previousTile, Tile newTile);

    /// <summary>
    /// Contains information about the dungeon the character is in
    /// </summary>
    [AddComponentMenu("DunGen/Character")]
    public class DungenCharacter : MonoBehaviour
    {
		#region Statics

		public static event DungenCharacterDelegate CharacterAdded;
		public static event DungenCharacterDelegate CharacterRemoved;

		public static ReadOnlyCollection<DungenCharacter> AllCharacters { get; private set; }
		private static readonly List<DungenCharacter> allCharacters = new List<DungenCharacter>();

		static DungenCharacter()
		{
			AllCharacters = new ReadOnlyCollection<DungenCharacter>(allCharacters);
		}

		#endregion

		public Tile CurrentTile { get { return currentTile; } set { currentTile = value; } }
        public event CharacterTileChangedEvent OnTileChanged;

        [SerializeField, HideInInspector]
        private Tile currentTile;


		protected virtual void OnEnable()
		{
			allCharacters.Add(this);

			if (CharacterAdded != null)
				CharacterAdded(this);
		}

		protected virtual void OnDisable()
		{
			allCharacters.Remove(this);

			if (CharacterRemoved != null)
				CharacterRemoved(this);
		}

        internal void ForceRecheckTile()
        {
            foreach(var tile in Component.FindObjectsOfType<Tile>())
                if (tile.Placement.Bounds.Contains(transform.position))
                {
                    HandleTileChange(tile);
                    break;
                }
        }

        protected virtual void OnTileChangedEvent(Tile previousTile, Tile newTile) { }

        internal void HandleTileChange(Tile newTile)
        {
            if (currentTile == newTile)
                return;

            var previousTile = currentTile;
            currentTile = newTile;

            if (OnTileChanged != null)
                OnTileChanged(this, previousTile, newTile);

            OnTileChangedEvent(previousTile, newTile);
        }
    }
}
