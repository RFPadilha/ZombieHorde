using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{
    public class ZombieMono : MonoBehaviour
    {
        public float RiseRate;
        public float walkSpeed;
        public GameObject zombieAnimationPrefab;
        public float maxHealth;
        public float damage;
    }
    public class ZombieBaker : Baker<ZombieMono>
    {
        public override void Bake(ZombieMono authoring)
        {
            //baking prerequisites
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new ZombieAnimationComponent { value = authoring.zombieAnimationPrefab });

            //actual baking, adding components much like we add components to gameObjects
            AddComponent(entity, new ZombieRiseRate { value = authoring.RiseRate });
            AddComponent(entity, new ZombieWalkProperties { walkSpeed = authoring.walkSpeed });
            AddComponent<ZombieHeading>(entity);
            AddComponent<NewZombieTag>(entity);


            AddComponent(entity, new ZombieDamageProperties
            {
                damagePerSecond = authoring.damage
            });

        }
    }
}