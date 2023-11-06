using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


namespace chillestCapybara
{
    [BurstCompile]
    public partial struct SpawnZombieSystem : ISystem
    {
        //All systems implement "OnCreate", "OnDestroy" and "OnUpdate" methods

        [BurstCompile]
        public void OnCreate(ref SystemState state) { 
        }



        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }



        [BurstCompile]
        public void OnStartRunning(ref SystemState state) 
        {
        }



        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            float deltaTime = SystemAPI.Time.DeltaTime;


            BeginInitializationEntityCommandBufferSystem.Singleton ecbSingleton = 
                SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            

            new SpawnZombieJob
            {
                dt = deltaTime,
                ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule();
        }
    }


    //Jobs are Unity's multithreading methods
    //Jobs are always scheduled by the main thread, even though worker threads will execute some of them
    [BurstCompile]
    public partial struct SpawnZombieJob : IJobEntity
    {
        public float dt;
        public EntityCommandBuffer ecb;
        private void Execute(GraveyardAspect graveyard)
        {
            graveyard.ZombieSpawnTimer -= dt;
            if (!graveyard.TimeToSpawnZombie) return;
            if (!graveyard.ZombieSpawnPointInitialized()) return;

            graveyard.ZombieSpawnTimer = graveyard.ZombieSpawnRate;
            Entity newZombie = ecb.Instantiate(graveyard.ZombiePrefab);

            LocalTransform newZombieTransform = graveyard.GetZombieSpawnPoint();
            ecb.SetComponent(newZombie, newZombieTransform);

            float zombieHeading = MathHelpers.GetHeading(newZombieTransform.Position, graveyard.Position);
            ecb.SetComponent(newZombie, new ZombieHeading { value =  zombieHeading });
        }
    }
}