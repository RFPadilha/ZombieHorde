﻿using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace chillestCapybara
{
    public partial struct ProjectileMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, moveSpeed) in SystemAPI.Query<RefRW<LocalTransform>, ProjectileMoveSpeed>())
            {
                transform.ValueRW.Position += transform.ValueRO.Forward() * moveSpeed.Value * deltaTime;
            }
        }
    }
}