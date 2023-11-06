using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace chillestCapybara
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        public GameObject ProjectilePrefab;
        public GameObject playerAnimationPrefab;
        public float health;
    }

    public class PlayerAuthoringBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);


            AddComponent(playerEntity, new PlayerHealthComponent
            {
                maxValue = authoring.health,
                currentValue = authoring.health
            });

            AddComponentObject(playerEntity, new PlayerAnimationComponent { value = authoring.playerAnimationPrefab });

            AddComponent<PlayerTag>(playerEntity);
            AddComponent<PlayerMoveInput>(playerEntity);
            AddComponent<PlayerRotationComponent>(playerEntity);
            
            AddComponent<FireProjectileTag>(playerEntity);
            SetComponentEnabled<FireProjectileTag>(playerEntity, false);
            
            AddComponent(playerEntity, new PlayerMoveSpeed
            {
                value = authoring.MoveSpeed
            });
            AddComponent(playerEntity, new ProjectilePrefab
            {
                value = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic)
            });
            AddBuffer<DamageBufferElement>(playerEntity);
            AddComponent<PlayerScoreComponent>(playerEntity);
        }
    }
}