using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct PlayerMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            new PlayerMoveJob()
            {
                dt = deltaTime
            }.Schedule();
        }
    }
    [BurstCompile]
    public partial struct PlayerMoveJob : IJobEntity
    {
        public float dt;
        [BurstCompile]
        private void Execute(ref LocalTransform transform, in PlayerMoveInput moveInput, PlayerMoveSpeed moveSpeed)
        {
            transform.Position.x += moveInput.value.x * moveSpeed.value * dt;
            transform.Position.z += moveInput.value.y * moveSpeed.value * dt;
            /*
            if (math.lengthsq(moveInput.value) > float.Epsilon)
            {
                float3 forward = new float3(moveInput.value.x, 0f, moveInput.value.y);
                transform.Rotation = quaternion.LookRotation(forward, math.up());
            }
            */
        }
    }
}