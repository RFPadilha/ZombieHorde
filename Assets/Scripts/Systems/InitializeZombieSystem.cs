using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;

namespace chillestCapybara
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitializeZombieSystem : ISystem
    {

        //All systems implement "OnCreate", "OnDestroy" and "OnUpdate" methods
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }
        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (ZombieWalkAspect zombie in SystemAPI.Query<ZombieWalkAspect>().WithAll<NewZombieTag>())
            {
                ecb.RemoveComponent<NewZombieTag>(zombie.entity);
                ecb.SetComponentEnabled<ZombieWalkProperties>(zombie.entity, false);
                ecb.SetComponentEnabled<ZombieDamageProperties>(zombie.entity, false);
            }
            ecb.Playback(state.EntityManager);
        }
    }
}