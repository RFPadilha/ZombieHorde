using Unity.Entities;
using UnityEngine;

namespace chillestCapybara
{
    public class ProjectileAuthoring : MonoBehaviour
    {
        public float ProjectileMoveSpeed;
        public float damage;
        public float timeToDestroy;

        public class ProjectileMoveSpeedBaker : Baker<ProjectileAuthoring>
        {
            public override void Bake(ProjectileAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ProjectileTimer
                {
                    value = authoring.timeToDestroy
                });
                AddComponent(entity, new ProjectileMoveSpeed { Value = authoring.ProjectileMoveSpeed });
                AddComponent(entity, new ProjectileDamageComponent
                {
                    damagePerShot = authoring.damage
                });
            }
        }
    }
}