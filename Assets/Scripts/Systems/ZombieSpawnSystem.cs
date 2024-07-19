using Aspects;
using ComponentsAndTags;
using Helpers;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(ZombieUpdateGroup))]
    public partial struct ZombieSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GraveyardComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            new ZombieSpawnJobs
            {
                deltaTime = deltaTime,
                ecb = ecb
            }.Schedule();
        }
    }

    [BurstCompile]
    public partial struct ZombieSpawnJobs : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer ecb;

        [BurstCompile]
        private void Execute(GraveyardAspect graveyardAspect)
        {
            graveyardAspect.ZombieSpawnTimer -= deltaTime;
            if(graveyardAspect.IsZombieSpawn == false) return;
            if(graveyardAspect.SpawnPointInit() == false) return;

            graveyardAspect.ZombieSpawnTimer = graveyardAspect.ZombieSpawnRate;
            var zombieInit = ecb.Instantiate(graveyardAspect.ZombiePrefab);
            var newZombieSpawnPoint = graveyardAspect.GetZombieSpawnPoint();
            ecb.SetComponent(zombieInit, newZombieSpawnPoint);

            var zombieHeading = MathHelpers.GetHeading(newZombieSpawnPoint.Position, graveyardAspect.Position);
            ecb.SetComponent(zombieInit, new ZombieHeading { angle = zombieHeading });
        }
    }
}