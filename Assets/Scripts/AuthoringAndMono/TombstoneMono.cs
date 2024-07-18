using ComponentsAndTags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
    public class TombstoneMono : MonoBehaviour
    {
        [SerializeField] private GameObject TombstoneRenderer;

        private class TombstoneMonoBaker : Baker<TombstoneMono>
        {
            public override void Bake(TombstoneMono authoring)
            {
                var rendererTombstone = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(rendererTombstone, new TombstoneRenderer 
                    { prefab = GetEntity(authoring.TombstoneRenderer, TransformUsageFlags.Dynamic) });
            }
        }
    }
}