using ComponentsAndTags;
using Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
    public class ZombieMono : MonoBehaviour
    {
        [SerializeField] private float _zombieRiseTime;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float walkAmplitude;
        [SerializeField] private float walkFrequency;

        private class ZombieMonoBaker : Baker<ZombieMono>
        {
            public override void Bake(ZombieMono authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ZombieRiseTime { timeToRise = authoring._zombieRiseTime });
                AddComponent(entity, new ZombieWalkProperties 
                {   walkSpeed = authoring.walkSpeed, 
                    walkFrequency = authoring.walkFrequency, 
                    walkAmplitude = authoring.walkAmplitude 
                });
                AddComponent(entity, new ZombieTimer());
                AddComponent(entity, new ZombieHeading());
                AddComponent(entity, new ZombieTag());
            }
        }
    }
}