using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{

    public class ZombieAnimationComponent : IComponentData
    {
        public GameObject value;
    }
    public class ZombieAnimatorReference : ICleanupComponentData
    {
        public Animator value;
    }
}
