using Unity.Entities;
using Unity.Mathematics;

namespace ComponentsAndTags
{
    public struct ZombieSpawnPoints : IComponentData
    {
        public BlobAssetReference<ZombieSpawnPointsBlob> spawnPoint;
    }

    public struct ZombieSpawnPointsBlob : IComponentData
    {
        public BlobArray<float3> spawnPoint;
    }
}