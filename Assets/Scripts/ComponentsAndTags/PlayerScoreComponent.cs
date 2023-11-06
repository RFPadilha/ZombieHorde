using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{
    public struct PlayerScoreComponent : IComponentData
    {
        public float score;
    }
}