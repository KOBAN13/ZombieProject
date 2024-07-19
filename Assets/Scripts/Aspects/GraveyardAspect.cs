using System;
using ComponentsAndTags;
using Helpers;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.SocialPlatforms;

namespace Aspects
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity TombstonesPrefab;
        private readonly RefRO<GraveyardComponent> _graveyardComponent;
        private readonly RefRW<GraveyardRandom> _graveyardRandom;
        private readonly RefRO<LocalTransform> _localTransform;
        private readonly RefRW<ZombieSpawnPoints> _zombieSpawnPoints;
        private readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimer;
    
        private const float BRAINT_SAFETY_RADIUS_SQ = 100;
        public int NumberTombstoneSpawn => _graveyardComponent.ValueRO.countTombstonesToSpawn;
        public Entity EntityTombstone => _graveyardComponent.ValueRO.tombstonesPrefab;
        

        public LocalTransform GetRandomTombstoneTransform()
        {
            return new LocalTransform
            {
                Position = RandomPosition(),
                Rotation = RandomRotate(),
                Scale = 1f
            };
        }

        public float3 Position => _localTransform.ValueRO.Position;

        public float ZombieSpawnTimer
        {
            get => _zombieSpawnTimer.ValueRO.timerSpawnZombie;
            set => _zombieSpawnTimer.ValueRW.timerSpawnZombie = value;
        }

        public Entity ZombiePrefab => _graveyardComponent.ValueRO.zombiePrefab;
        public float ZombieSpawnRate => _graveyardComponent.ValueRO.timerateZombieSpawn;

        public bool IsZombieSpawn => _zombieSpawnTimer.ValueRO.timerSpawnZombie <= 0;
        
        public float2 RandomMaterialParameters()
        {
            return _graveyardRandom.ValueRW.Random.NextFloat2(new float2(0f, 0f), new float2(1f, 1f));
        }

        public bool SpawnPointInit()
        {
            return _zombieSpawnPoints.ValueRO.spawnPoint.IsCreated && ZombieSpawnPointCount > 0;
        }

        public LocalTransform GetZombieSpawnPoint()
        {
            var position = GetRandomZombieSpawnPoint();
            return new LocalTransform
            {
                Position = position,
                Rotation = quaternion.RotateY(MathHelpers.GetHeading(position, _localTransform.ValueRO.Position)),
                Scale = 1f
            };
        }

        private int ZombieSpawnPointCount => _zombieSpawnPoints.ValueRO.spawnPoint.Value.spawnPoint.Length;

        private float3 GetRandomZombieSpawnPoint()
        {
            return GetZombieSpawn(_graveyardRandom.ValueRW.Random.NextInt(ZombieSpawnPointCount));
        }
        
        private float3 GetZombieSpawn(int i) => _zombieSpawnPoints.ValueRO.spawnPoint.Value.spawnPoint[i];

        private float3 RandomPosition()
        {
            float3 randomTransform;
            do
            {
                randomTransform = _graveyardRandom.ValueRW.Random.NextFloat3(MinCorner, MaxCorner);
            } 
            while (math.distancesq(_localTransform.ValueRO.Position, randomTransform) < BRAINT_SAFETY_RADIUS_SQ);

            return randomTransform;
        }

        private quaternion RandomRotate()
        {
            return quaternion.RotateY(_graveyardRandom.ValueRW.Random.NextFloat(-0.5f, 0.5f));
        }

        private float3 MinCorner => _localTransform.ValueRO.Position - HalfDimension;
        private float3 MaxCorner => _localTransform.ValueRO.Position + HalfDimension;

        private float3 HalfDimension => new()
        {
            x = _graveyardComponent.ValueRO.fieldDimention.x * 0.5f,
            y = 0f,
            z = _graveyardComponent.ValueRO.fieldDimention.y * 0.5f,
        };
    }
}