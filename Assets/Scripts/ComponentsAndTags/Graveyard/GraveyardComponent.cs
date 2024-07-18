using Unity.Entities;
using Unity.Mathematics;

public struct GraveyardComponent : IComponentData
{
    public float2 fieldDimention;
    public int countTombstonesToSpawn;
    public Entity tombstonesPrefab;
    public Entity zombiePrefab;
    public float timerateZombieSpawn;
}
