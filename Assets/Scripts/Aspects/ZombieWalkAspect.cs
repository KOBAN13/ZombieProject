﻿using ComponentsAndTags;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Aspects
{
    public readonly partial struct ZombieWalkAspect : IAspect
    {
        private readonly RefRO<ZombieWalkProperties> _zombieWalkProperties;
        private readonly RefRW<ZombieTimer> _zombieTimer;
        private readonly RefRW<ZombieHeading> _zombieHeading;
        private readonly RefRW<LocalTransform> _zombieTransform;

        private float ZombieSpeed => _zombieWalkProperties.ValueRO.walkSpeed;
        private float ZombieAmplitude => _zombieWalkProperties.ValueRO.walkAmplitude;
        private float ZombieFrequency => _zombieWalkProperties.ValueRO.walkFrequency;

        private float ZombieTimer
        {
            get => _zombieTimer.ValueRO.timer;
            set => _zombieTimer.ValueRW.timer = value;
        }

        public void Walk(float deltaTime)
        {
            ZombieTimer += deltaTime;
            _zombieTransform.ValueRW.Position += _zombieTransform.ValueRW.Forward() * ZombieSpeed * deltaTime;
        }
    }
}