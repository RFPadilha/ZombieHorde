using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{
    public struct ProjectileDamageComponent : IComponentData, IEnableableComponent
    {
        public float damagePerShot;
    }
}