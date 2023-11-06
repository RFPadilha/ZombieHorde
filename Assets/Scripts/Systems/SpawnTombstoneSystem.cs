using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace chillestCapybara
{
    //Systems are what actually use the data contained and manipulated by components(held by entities) to implement the game logic
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GraveyardProperties>();//makes sure there is a "GraveyardProperties" component before running update
        }


        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

            state.Enabled = false;//makes update run only once
            float3 tombstoneOffset = new float3(0f, -4f, .5f);


            Entity graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
            GraveyardAspect graveyard = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);//initializes graveyard


            EntityCommandBuffer ecb = 
                new EntityCommandBuffer(Allocator.Temp);//stores queue of thread safe commands, which must be "played back" to apply



            //Usage of blob assets
            //1 - Create a BlobBuilder.This needs to allocate some memory internally.
            BlobBuilder builder = new BlobBuilder(Allocator.Temp);
            //2 - Construct the root of the blob asset using BlobBuilder.ConstructRoot
            ref ZombieSpawnPointsBlob spawnPoints = ref builder.ConstructRoot<ZombieSpawnPointsBlob>();



            //3 - Fill the structure with your data.
            BlobBuilderArray<float3> arrayBuilder = 
                builder.Allocate(ref spawnPoints.Value, graveyard.tombsToSpawn);//builds spawn points array with specified length

            for (int i = 0; i < graveyard.tombsToSpawn; i++)
            {
                Entity newTombstone = ecb.Instantiate(graveyard.tombstonePrefab);//creates tombstone
                LocalTransform newTombstoneTransform = graveyard.GetRandomTombstoneTransform();//calculates position
                ecb.SetComponent(newTombstone, newTombstoneTransform);//sets position
                
                float3 newZombieSpawnPoint = newTombstoneTransform.Position + tombstoneOffset;//offsets zombie spawning position
                arrayBuilder[i] = newZombieSpawnPoint;//adds zombie spawn to array
            }



            //4 - This copies the blob asset to the final location.
            BlobAssetReference<ZombieSpawnPointsBlob> blobAsset = builder.CreateBlobAssetReference<ZombieSpawnPointsBlob>(Allocator.Persistent);
            ecb.SetComponent(graveyardEntity, new ZombieSpawnPoints { Value = blobAsset });

            //5 - Dispose the blob builder allocated in step 1.
            builder.Dispose();
            ecb.Playback(state.EntityManager);
        }

    }
}