using ComponentsAndTags.Brain;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Aspects
{
    public readonly partial struct BrainAspect : IAspect
    {
        private readonly RefRW<LocalTransform> brainTransform;
        private readonly RefRO<BrainProperties> brainProperties;

        public float3 BrainTransform => brainTransform.ValueRW.Position;
    }
}