using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    public readonly partial struct PlayerAspect : IAspect
    {
        public readonly Entity entity;
        public readonly RefRW<LocalTransform> _transform;
        public readonly RefRW<PlayerHealthComponent> _playerHealth;
        private readonly DynamicBuffer<DamageBufferElement> _damageBuffer;

        private readonly RefRO<PlayerMoveInput> _moveInput;
        private readonly RefRO<PlayerMoveSpeed> _moveSpeed;
        private readonly RefRO<PlayerRotationComponent> _rotationComponent;
        private readonly RefRW<PlayerScoreComponent> _scoreComponent;
        private readonly RefRO<PlayerTag> _tag;

        public float3 Position => _transform.ValueRO.Position;

        public void DamagePlayer()
        {
            foreach(var damageBufferElement in _damageBuffer)
            {
                _playerHealth.ValueRW.currentValue -= damageBufferElement.value;
            }
            _damageBuffer.Clear();
        }
        public void IncreaseScore()
        {
            _scoreComponent.ValueRW.score += 1;
        }
    }
}