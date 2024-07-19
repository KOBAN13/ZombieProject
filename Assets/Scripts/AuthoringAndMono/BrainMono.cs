using ComponentsAndTags.Brain;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
    public class BrainMono : MonoBehaviour
    {
        [SerializeField] private float radiusBrain;

        private class BrainMonoBaker : Baker<BrainMono>
        {
            public override void Bake(BrainMono authoring)
            {
                Entity brain = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(brain, new BrainProperties { radiusBrain = authoring.radiusBrain });
            }
        }
    }
}