using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    public readonly partial struct ProjectileAspect : IAspect
    {

        public readonly Entity entity;
        public readonly RefRO<LocalTransform> _transform;
        private readonly RefRO<ProjectileDamageComponent> _damageComponent;
        private readonly RefRW<ProjectileTimer> _timer;

        private float damagePerShot => _damageComponent.ValueRO.damagePerShot;

        public void CountDown(float dt, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity entity)
        {
            if (_timer.ValueRO.value > 0)
            {
                _timer.ValueRW.value -= dt;
            }
            else
            {
                ecb.DestroyEntity(sortKey, entity);
            }
        }

        public void Damage(EntityCommandBuffer.ParallelWriter ecb, int sortkey, Entity zombieEntity)
        {
            var curDamage = new DamageBufferElement { value = damagePerShot };
            ecb.AppendToBuffer(sortkey, zombieEntity, curDamage);
        }
        public bool IsInDamageRange(float3 targetPosition, float distRadiusSq)
        {
            return math.distancesq(targetPosition, _transform.ValueRO.Position) <= distRadiusSq;
        }
    }
}