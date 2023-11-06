using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
    public partial struct ZombieAnimationSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (zombieAnimationPrefab, entity) in
                SystemAPI.Query<ZombieAnimationComponent>().WithNone<ZombieAnimatorReference>().WithEntityAccess())
            {
                GameObject newCompanionGameObject = Object.Instantiate(zombieAnimationPrefab.value);
                ZombieAnimatorReference newAnimatorReference = new ZombieAnimatorReference
                {
                    value = newCompanionGameObject.GetComponent<Animator>()

                };
                ecb.AddComponent(entity, newAnimatorReference);
            }

            foreach (var (transform, animatorReference) in
                SystemAPI.Query<LocalTransform, ZombieAnimatorReference>())
            {
                animatorReference.value.transform.position = transform.Position;
                animatorReference.value.transform.rotation = transform.Rotation;

            }

            foreach (var (animatorReference, entity) in
                SystemAPI.Query<ZombieAnimatorReference>().WithNone<ZombieAnimationComponent, LocalTransform>().WithEntityAccess())
            {
                //death animation here
                Object.Destroy(animatorReference.value.gameObject);
                ecb.RemoveComponent<ZombieAnimatorReference>(entity);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
