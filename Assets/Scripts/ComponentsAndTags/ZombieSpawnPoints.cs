using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace chillestCapybara
{
    //Blob stands for "Binary Large Object", used to represent immutable data in order to safely access in parallel jobs
    //Blob assets are used with a "BlobBuilder", so it manages the memory organization offsets for each asset
    //Blob assets must always be passed by reference using "ref"
    public struct ZombieSpawnPoints : IComponentData
    {
        public BlobAssetReference<ZombieSpawnPointsBlob> Value;
    }

    public struct ZombieSpawnPointsBlob
    {
        public BlobArray<float3> Value;
    }
    public struct ZombieSpawnTimer : IComponentData
    {
        public float value;
    }
}