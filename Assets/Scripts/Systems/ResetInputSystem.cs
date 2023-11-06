using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;

namespace chillestCapybara
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct ResetInputSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach(var (tag,entity) in SystemAPI.Query<FireProjectileTag>().WithEntityAccess())
            {
                ecb.SetComponentEnabled<FireProjectileTag>(entity, false);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
