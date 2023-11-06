using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

namespace chillestCapybara
{
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
    public partial struct PlayerAnimationSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach(var (playerAnimationPrefab, entity) in
                SystemAPI.Query<PlayerAnimationComponent>().WithNone<PlayerAnimatorReference>().WithEntityAccess())
            {
                GameObject newCompanionGameObject = Object.Instantiate(playerAnimationPrefab.value);
                PlayerAnimatorReference newAnimatorReference = new PlayerAnimatorReference
                {
                    value = newCompanionGameObject.GetComponent<Animator>()

                };
                ecb.AddComponent(entity, newAnimatorReference);
            }

            foreach ( var (transform, animatorReference, moveInput) in
                SystemAPI.Query<LocalTransform, PlayerAnimatorReference, PlayerMoveInput>())
            {
                animatorReference.value.SetBool("IsMoving", math.length(moveInput.value) > 0f);
                animatorReference.value.transform.position = transform.Position - new float3(0f,.6f,0);
                animatorReference.value.transform.rotation = transform.Rotation;
            }

            foreach(var(animatorReference,entity) in
                SystemAPI.Query<PlayerAnimatorReference>().WithNone<PlayerAnimationComponent, LocalTransform>().WithEntityAccess())
            {
                Object.Destroy(animatorReference.value.gameObject);
                ecb.RemoveComponent<PlayerAnimatorReference>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}