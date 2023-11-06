using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

namespace chillestCapybara
{
    //a combination of components and methods to manipulate data
    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity entity;
        private readonly RefRW<LocalTransform> _transform;
        private LocalTransform Transform => _transform.ValueRO;

        private readonly RefRO<GraveyardProperties> _graveyardProperties;
        private readonly RefRW<GraveyardRandom> _graveyardRandom;





        //Tombstones------------------------------------------------------------------------------------------------------
        public int tombsToSpawn => _graveyardProperties.ValueRO.tombstoneAmount;
        public Entity tombstonePrefab => _graveyardProperties.ValueRO.tombstonePrefab;

        public LocalTransform GetRandomTombstoneTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition() + new float3(0,5.4f,0),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale(.5f)
            };
        }





        //Zombies------------------------------------------------------------------------------------------------------
        private readonly RefRW<ZombieSpawnPoints> _zombieSpawnPoints;
        private readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimer;

        public LocalTransform GetZombieSpawnPoint()
        {
            float3 position = GetRandomZombieSpawnPoint();
            return new LocalTransform
            {
                Position = position,
                Rotation = quaternion.RotateY(MathHelpers.GetHeading(position, Transform.Position)),
                Scale = 1f
            };
        }
        private float3 GetRandomZombieSpawnPoint()
        {
            return GetZombieSpawnPoint(_graveyardRandom.ValueRW.value.NextInt(ZombieSpawnPointCount));
        }
        private float3 GetZombieSpawnPoint(int i) => _zombieSpawnPoints.ValueRO.Value.Value.Value[i];
        public bool ZombieSpawnPointInitialized()
        {
            return _zombieSpawnPoints.ValueRO.Value.IsCreated && ZombieSpawnPointCount > 0;
        }
        public float ZombieSpawnTimer
        {
            get => _zombieSpawnTimer.ValueRO.value;
            set => _zombieSpawnTimer.ValueRW.value = value;
        }
        public float3 Position => Transform.Position;
        private int ZombieSpawnPointCount => _zombieSpawnPoints.ValueRO.Value.Value.Value.Length;
        public bool TimeToSpawnZombie => ZombieSpawnTimer <= 0f;
        public float ZombieSpawnRate => _graveyardProperties.ValueRO.zombieSpawnRate;
        public Entity ZombiePrefab => _graveyardProperties.ValueRO.zombiePrefab;




        
        //Utils------------------------------------------------------------------------------------------------------
        private float3 GetRandomPosition()
        {
            float3 randomPos;
            do
            {
                randomPos = _graveyardRandom.ValueRW.value.NextFloat3(minCorner, maxCorner);
            } while (math.distancesq(Transform.Position + new float3(0, 5f, 0), randomPos) <= spawnSafetyRadius);
            return randomPos;
        }

        private float3 minCorner => Transform.Position - HalfDimensions;
        private float3 maxCorner => Transform.Position + HalfDimensions;
        private float3 HalfDimensions => new()
        {
            x = _graveyardProperties.ValueRO.fieldDimensions.x * .5f,
            y = 0f,
            z = _graveyardProperties.ValueRO.fieldDimensions.z * .5f
        };
        private const float spawnSafetyRadius = 4;
        private quaternion GetRandomRotation() => quaternion.RotateY(_graveyardRandom.ValueRW.value.NextFloat(-.25f, .25f));
        private float GetRandomScale(float min) => _graveyardRandom.ValueRW.value.NextFloat(min, 1f);

    }
}