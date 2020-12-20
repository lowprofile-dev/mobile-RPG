using UnityEngine;

namespace DunGen
{
    [AddComponentMenu("DunGen/Random Props/Random Prefab")]
    public class RandomPrefab : RandomProp
    {
        [AcceptGameObjectTypes(GameObjectFilter.Asset)]
        public GameObjectChanceTable Props = new GameObjectChanceTable();
        public bool ZeroPosition = true;
        public bool ZeroRotation = true;


        public override void Process(System.Random randomStream, Tile tile)
        {
            if (Props.Weights.Count <= 0)
                return;

            var chosenEntry = Props.GetRandom(randomStream, tile.Placement.IsOnMainPath, tile.Placement.NormalizedDepth, null, true, true);
            var prefab = chosenEntry.Value;

            GameObject newProp = (GameObject)GameObject.Instantiate(prefab);
            newProp.transform.parent = transform;

            if (ZeroPosition)
                newProp.transform.localPosition = Vector3.zero;
            else
                newProp.transform.localPosition = prefab.transform.localPosition;

            if (ZeroRotation)
                newProp.transform.localRotation = Quaternion.identity;
            else
                newProp.transform.localRotation = prefab.transform.localRotation;
        }
    }
}