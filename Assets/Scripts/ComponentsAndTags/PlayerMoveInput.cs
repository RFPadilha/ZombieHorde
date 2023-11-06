using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace chillestCapybara
{
    public struct PlayerMoveInput : IComponentData
    {
        public float2 value;
    }
    public struct  PlayerMoveSpeed : IComponentData
    {
        public float value;
    }
    public struct PlayerTag : IComponentData { }
    public struct FireProjectileTag : IComponentData, IEnableableComponent { }
    public struct ProjectilePrefab : IComponentData
    {
        public Entity value;
    }
}

