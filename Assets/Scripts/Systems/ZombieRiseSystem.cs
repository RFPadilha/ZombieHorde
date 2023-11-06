using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{
    [BurstCompile]
    [UpdateAfter(typeof(SpawnZombieSystem))]
    public partial struct ZombieRiseSystem : ISystem
    {
        //All systems implement "OnCreate", "OnDestroy" and "OnUpdate" methods
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }
        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton = 
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();


            new ZombieRiseJob
            {
                dt = deltaTime,
                ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }
    [BurstCompile]
    public partial struct ZombieRiseJob : IJobEntity
    {
        public float dt;
        public EntityCommandBuffer.ParallelWriter ecb;

        [BurstCompile]
        private void Execute(ZombieRiseAspect zombie, [ChunkIndexInQuery]int sortKey)
        {
            zombie.Rise(dt);
            if (!zombie.IsAboveGround) return;

            zombie.SetAtGroundLevel();
            ecb.RemoveComponent<ZombieRiseRate>(sortKey, zombie.entity);
            ecb.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.entity, true);
        }
    }
}