using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    public readonly partial struct ZombieWalkAspect : IAspect
    {
        public readonly Entity entity;

        public readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<ZombieWalkProperties> _walkProperties;
        private readonly RefRO<ZombieHeading> _heading;

        public float3 localPosition => _transform.ValueRO.Position;
        private float WalkSpeed => _walkProperties.ValueRO.walkSpeed;
        private float Heading => _heading.ValueRO.value;


        public void Walk(float dt, float3 target)
        {
            float faceTowards = MathHelpers.GetHeading(localPosition, target);
            _transform.ValueRW.Position += _transform.ValueRO.Forward() * WalkSpeed * dt;
            _transform.ValueRW.Rotation = quaternion.Euler(0, faceTowards, 0);
        }

        public bool IsInStoppingRange(float3 targetPosition, float distRadiusSq)
        {
            return math.distancesq(targetPosition, _transform.ValueRO.Position) <= distRadiusSq;
        }
    }
}