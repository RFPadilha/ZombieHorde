using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    public readonly partial struct ZombieRiseAspect : IAspect
    {
        public readonly Entity entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<ZombieRiseRate> _zombieRiseRate;

        public void Rise(float deltaTime)
        {
            _transform.ValueRW.Position += math.up() * _zombieRiseRate.ValueRO.value * deltaTime;
        }
        public bool IsAboveGround => _transform.ValueRO.Position.y >= 5f;
        public void SetAtGroundLevel()
        {
            float3 position = _transform.ValueRO.Position;
            position.y = 5f;
            _transform.ValueRW.Position = position;
        }
    }
}