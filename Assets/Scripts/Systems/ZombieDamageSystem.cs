using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial struct ZombieDamageSystem : ISystem
    {
        Entity player;
        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
            state.RequireForUpdate<PlayerMoveInput>();//makes sure there is a "PlayerMoveInput" component before running update
            EntityQuery query = state.GetEntityQuery(typeof(PlayerMoveInput));
            state.RequireForUpdate(query);
        }
        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            player = SystemAPI.GetSingletonEntity<PlayerMoveInput>();
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            new ZombieDamageJob
            {
                dt= deltaTime,
                ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                playerEntity = player
            }.ScheduleParallel();
        }

    }
    [BurstCompile]
    public partial struct ZombieDamageJob : IJobEntity
    {
        public float dt;
        public EntityCommandBuffer.ParallelWriter ecb;
        public Entity playerEntity;
        
        [BurstCompile]
        private void Execute(ZombieDamageAspect zombie, [ChunkIndexInQuery] int sortKey)
        {
            zombie.Damage(dt, ecb, sortKey, playerEntity);
        }
    }
}