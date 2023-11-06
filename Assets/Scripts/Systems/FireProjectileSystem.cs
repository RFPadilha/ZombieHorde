using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct FireProjectileSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach(var (projectilePrefab, transform) in 
                SystemAPI.Query<ProjectilePrefab, LocalTransform>().WithAll<FireProjectileTag>())
            {
                Entity newProjectile = ecb.Instantiate(projectilePrefab.value);
                LocalTransform projectileTransform = LocalTransform.FromPositionRotation(transform.Position, transform.Rotation);
                ecb.SetComponent(newProjectile, projectileTransform);
                ecb.SetComponentEnabled<ProjectileDamageComponent>(newProjectile, true);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
        
    }
}
