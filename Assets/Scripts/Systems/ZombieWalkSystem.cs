using chillestCapybara;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

namespace chillestCapybara
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup),OrderLast = true)]
    public partial class ZombieWalkSystem : SystemBase
    {
        Entity playerEntity;
        float3 playerPos;
        //All systems implement "OnCreate", "OnDestroy" and "OnUpdate" methods
        [BurstCompile]
        protected override void OnCreate() { }
        [BurstCompile]
        protected override void OnDestroy() { }
        protected override void OnStartRunning()
        {
            EntityQuery query = GetEntityQuery(typeof(PlayerMoveInput));
            RequireForUpdate(query);
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton = 
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            playerEntity = SystemAPI.GetSingletonEntity<PlayerMoveInput>();

            playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

            new ZombieWalkJob
            {
                dt = deltaTime,
                distRadiusSq = 1,
                targetPos = playerPos,
                ecb = ecbSingleton.CreateCommandBuffer(World.Unmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }
    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity
    {
        public float dt;
        public float distRadiusSq;
        public float3 targetPos;
        public EntityCommandBuffer.ParallelWriter ecb;

        [BurstCompile]
        private void Execute(ZombieWalkAspect zombie, [ChunkIndexInQuery]int sortKey)
        {
            if(!zombie.IsInStoppingRange(targetPos, 1f))
            {
                zombie.Walk(dt, targetPos);
                ecb.SetComponentEnabled<ZombieDamageProperties>(sortKey, zombie.entity, false);
            }
            else
            {
                ecb.SetComponentEnabled<ZombieDamageProperties>(sortKey, zombie.entity, true);
            }
        }
    }
}
