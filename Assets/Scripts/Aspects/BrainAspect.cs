using ComponentsAndTags.Brain;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Aspects
{
    public readonly partial struct BrainAspect : IAspect
    {
        public readonly Entity brain;
        
        private readonly RefRW<LocalTransform> brainTransform;
        private readonly RefRW<BrainProperties> brainProperties;
        private readonly DynamicBuffer<BrainDamageBufferElement> _brainDamageBuffer;

        public float3 BrainTransform => brainTransform.ValueRW.Position;
        public float Scale => brainTransform.ValueRO.Scale;

        public void DamageBrain()
        {
            foreach (var brainDamageBufferElement in _brainDamageBuffer)
            {
                brainProperties.ValueRW.Value -= brainDamageBufferElement.damage;
            }
            _brainDamageBuffer.Clear();
            brainTransform.ValueRW.Scale = brainProperties.ValueRO.Value / brainProperties.ValueRO.MaxHealth;
        }
    }
}