using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;


namespace chillestCapybara
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class CameraTargetSystem : SystemBase
    {
        private Entity _playerEntity;
        private float3 offset = new float3(0f, 15f, 0f);
        private Camera mainCamera;


        protected override void OnStartRunning()
        {
            EntityQuery query = GetEntityQuery(typeof(PlayerMoveInput));
            RequireForUpdate(query);
        }
        protected override void OnUpdate()
        {
            mainCamera = Camera.main;
            _playerEntity = SystemAPI.GetSingletonEntity<PlayerMoveInput>();
            mainCamera.transform.position = SystemAPI.GetComponent<LocalTransform>(_playerEntity).Position + offset;
        }
    }
}