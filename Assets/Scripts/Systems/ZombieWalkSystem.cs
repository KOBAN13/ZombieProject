using Aspects;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(ZombieUpdateGroup), OrderLast = true)]
    public partial struct ZombieWalkSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ZombieWalkJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity
    {
        public float deltaTime;
        [BurstCompile]
        private void Execute(ZombieWalkAspect zombieWalkAspect)
        {
            zombieWalkAspect.Walk(deltaTime);
        }
    }
}