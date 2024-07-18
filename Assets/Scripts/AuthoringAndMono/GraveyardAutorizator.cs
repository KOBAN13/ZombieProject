using ComponentsAndTags;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class GraveyardAutorizator : MonoBehaviour
{
    [SerializeField] private float2 fieldDimention;
    [SerializeField] private int countTombstonesToSpawn;
    [SerializeField] private GameObject tombstonesPrefab;
    [SerializeField] private uint randomSeed;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float timerateZombieSpawn;

    private class GraveyardAutorizatorBaker : Baker<GraveyardAutorizator>
    {
        public override void Bake(GraveyardAutorizator authoring)
        {
            Entity tombstonesPrefab = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(tombstonesPrefab, new GraveyardComponent
            {
                countTombstonesToSpawn = authoring.countTombstonesToSpawn,
                fieldDimention = authoring.fieldDimention,
                tombstonesPrefab = GetEntity(authoring.tombstonesPrefab, TransformUsageFlags.Dynamic),
                zombiePrefab = GetEntity(authoring.zombiePrefab, TransformUsageFlags.Dynamic),
                timerateZombieSpawn = authoring.timerateZombieSpawn
            });
            
            AddComponent(tombstonesPrefab,
                new GraveyardRandom { Random = Random.CreateFromIndex(authoring.randomSeed)});

            AddComponent<ZombieSpawnPoints>(tombstonesPrefab);
            AddComponent<ZombieSpawnPointsBlob>(tombstonesPrefab);
            AddComponent<ZombieSpawnTimer>(tombstonesPrefab);
        }
    }
} 
