using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace chillestCapybara
{
    public static class MathHelpers
    {
        public static float GetHeading(float3 objectPos, float3 targetPos)
        {
            float x = objectPos.x - targetPos.x;
            float y = objectPos.z - targetPos.z;
            return math.atan2(x, y) + math.PI;
        }
    }
}