using Unity.Entities;
using Unity.Mathematics;

namespace chillestCapybara
{
    //holds raw data to be used by entities and systems
    public struct GraveyardProperties : IComponentData
    {
        public int tombstoneAmount;

        public float zombieSpawnRate;
        public float3 fieldDimensions;

        public Entity tombstonePrefab;
        public Entity zombiePrefab;
    }
}