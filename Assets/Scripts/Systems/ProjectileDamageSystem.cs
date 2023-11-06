using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    [BurstCompile]
    [UpdateAfter(typeof(ProjectileMoveSystem))]
    public partial struct ProjectileDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnStartRunning(ref SystemState state) 
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float dt = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);


            //use jobs if possible
            foreach (var (projectile, transform) in 
                SystemAPI.Query<ProjectileAspect, LocalTransform>())
            {
                bool collided = false;
                foreach (var(zombie, zombieTransform) in 
                    SystemAPI.Query<ZombieWalkAspect, LocalTransform>())
                {
                    if(math.distance(transform.Position, zombieTransform.Position) < 1f)
                    {
                        ecbSingleton.DestroyEntity(zombie.entity);
                        //instead of destroying, reference the aspect to countdown health
                        //then if health is 0, play die animation
                        //then destroy
                        ecbSingleton.DestroyEntity(projectile.entity);
                        new IncreaseScoreJob { }.Schedule();
                        collided = true;
                        break;
                    }
                }
                if (collided) break;
            }
            new ProjectileDestroyJob
            {
                deltaTime = dt,
                ecb = ecbSingleton.AsParallelWriter(),
                timeToDestroy = 1f
            }.ScheduleParallel();
        }

    }
    [BurstCompile]
    [WithAll(typeof(ProjectileMoveSpeed))]
    public partial struct ProjectileDestroyJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public float timeToDestroy;
        [BurstCompile]
        private void Execute(ProjectileAspect projectile, [ChunkIndexInQuery] int sortKey)
        {
            projectile.CountDown(deltaTime, ecb, sortKey, projectile.entity);
        }
    }
    [BurstCompile]
    public partial struct IncreaseScoreJob : IJobEntity
    {
        private void Execute(PlayerAspect player)
        {
            player.IncreaseScore();
        }
    }
}