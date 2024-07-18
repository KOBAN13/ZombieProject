using Aspects;
using ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem : ISystem
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
            var graveyard = SystemAPI.GetSingletonEntity<GraveyardComponent>();
            var graveyardAspect = SystemAPI.GetAspect<GraveyardAspect>(graveyard);
            
            var tombstoneOffset = new float3(0f, -2f, 1f);
            
            using var builder = new BlobBuilder(Allocator.Temp);
            ref var spawnPoint = ref builder.ConstructRoot<ZombieSpawnPointsBlob>();
            var arrayBuilder = builder.Allocate(ref spawnPoint.spawnPoint, graveyardAspect.NumberTombstoneSpawn);
            
            using EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            for (var i = 0; i < graveyardAspect.NumberTombstoneSpawn; i++)
            {
                var transformTombstone = graveyardAspect.GetRandomTombstoneTransform();
                Entity tombstone = ecb.Instantiate(graveyardAspect.EntityTombstone);
                ecb.SetComponent(tombstone, transformTombstone);

                arrayBuilder[i] = transformTombstone.Position + tombstoneOffset;
            }

            var blobAsset = builder.CreateBlobAssetReference<ZombieSpawnPointsBlob>(Allocator.Persistent);
            ecb.SetComponent(graveyard, new ZombieSpawnPoints { spawnPoint = blobAsset });
            
            ecb.Playback(state.EntityManager);
        }
    }
}
