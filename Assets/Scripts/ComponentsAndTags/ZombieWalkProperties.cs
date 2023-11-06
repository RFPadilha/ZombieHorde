using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{
    public struct ZombieWalkProperties : IComponentData, IEnableableComponent
    {
        public float walkSpeed;
    }
    public struct ZombieHeading : IComponentData
    {
        public float value;
    }
    public struct NewZombieTag : IComponentData { }
    public struct  ZombieDamageProperties : IComponentData, IEnableableComponent
    {
        public float damagePerSecond;
    }
}