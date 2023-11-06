using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    public readonly partial struct ZombieDamageAspect : IAspect
    {
        public readonly Entity entity;
        private readonly RefRO<LocalTransform> _transform;
        private readonly RefRO<ZombieDamageProperties> _damageProperties;
        private readonly RefRO<ZombieHeading> _heading;

        private float damagePerSecond => _damageProperties.ValueRO.damagePerSecond;

        public void Damage(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortkey, Entity playerEntity)
        {
            var curDamage = new DamageBufferElement { value = damagePerSecond * deltaTime };
            ecb.AppendToBuffer(sortkey,playerEntity,curDamage);
        }
    }
}