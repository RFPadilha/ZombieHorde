using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace chillestCapybara
{
    //the only object attached via editor, used to add components to the relevant entity
    public class GraveyardMono : MonoBehaviour
    {

        public int tombstoneAmount;
        public uint randomSeed;

        public float zombieSpawnRate;
        public float3 fieldDimensions;

        public GameObject tombstonePrefab;
        public GameObject zombiePrefab;
    }


    //baking is the process through which a gameObject is converted into an entity
    public class GraveyardBaker : Baker<GraveyardMono>
    {
        public override void Bake(GraveyardMono authoring)
        {
            //baking prerequisites
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            //actual baking, adding components much like we add components to gameObjects
            AddComponent(entity, new GraveyardProperties
            {
                fieldDimensions = authoring.fieldDimensions,
                tombstoneAmount = authoring.tombstoneAmount,
                tombstonePrefab = GetEntity(authoring.tombstonePrefab, TransformUsageFlags.Dynamic),
                zombiePrefab = GetEntity(authoring.zombiePrefab, TransformUsageFlags.Dynamic),
                zombieSpawnRate = authoring.zombieSpawnRate

            });
            AddComponent(entity, new GraveyardRandom
            {
                value = Unity.Mathematics.Random.CreateFromIndex(authoring.randomSeed)
            });
            AddComponent<ZombieSpawnPoints>(entity);
            AddComponent<ZombieSpawnTimer>(entity);
        }

    }
}
