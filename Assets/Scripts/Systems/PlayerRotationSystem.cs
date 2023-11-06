using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Physics;
using Unity.Physics.Systems;

namespace chillestCapybara
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class PlayerRotationSystem : SystemBase
    {
        public float3 mousePos;
        private Camera mainCamera;

        protected override void OnStartRunning()
        {
        }

        protected override void OnUpdate()
        {
            mainCamera = Camera.main;
            var mousePosition = Mouse.current.position.ReadValue();
            var cameraRay = mainCamera.ScreenPointToRay(mousePosition);
            var layerMask = LayerMask.GetMask("Floor");

            if (Physics.Raycast(cameraRay, out var hit, 100, layerMask))
            {
                mousePos = (float3)hit.point;
            }
            new PlayerRotateJob
            {
                lookAtPoint = mousePos
            }.Schedule();

        }
    }
    [BurstCompile]
    public partial struct PlayerRotateJob : IJobEntity
    {
        public float3 lookAtPoint;
        [BurstCompile]
        private void Execute(ref LocalTransform transform, PlayerRotationComponent rotation)
        {
            lookAtPoint.y = transform.Position.y;
            transform.Rotation = quaternion.LookRotation(lookAtPoint - transform.Position, math.up());
        }
    }
}
