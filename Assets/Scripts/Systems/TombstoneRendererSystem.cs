using Aspects;
using ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SpawnTombstoneSystem))]
    public partial struct TombstoneRendererSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GraveyardComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var aspect = SystemAPI.GetAspect<GraveyardAspect>(SystemAPI.GetSingletonEntity<GraveyardComponent>());
            using var ecs = new EntityCommandBuffer(Allocator.Temp);

            foreach (var renderer in SystemAPI.Query<RefRW<TombstoneRenderer>>())
            {
                ecs.AddComponent(renderer.ValueRW.prefab, new TombstoneMaterialProperty { materialProperty = aspect.RandomMaterialParameters() });
            }
            
            ecs.Playback(state.EntityManager);
        }
    }
}