using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{
    public partial struct ZombieHealthComponent : IComponentData
    {
        public float maxValue;
        public float currentValue;
    }
}
