using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup),OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct PlayerHealthSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach(var player in SystemAPI.Query<PlayerAspect>())
            {
                player.DamagePlayer();
            }
        }
    }
}