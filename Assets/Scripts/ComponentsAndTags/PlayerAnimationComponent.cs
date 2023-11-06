using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{

    public class PlayerAnimationComponent : IComponentData
    {
        public GameObject value; 
    }
    public class PlayerAnimatorReference : ICleanupComponentData
    {
        public Animator value;
    }
}