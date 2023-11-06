using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace chillestCapybara
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class GetPlayerInputSystem : SystemBase
    {
        private PlayerInputActions _playerInputActions;
        private Entity _playerEntity;
        private float fireRate = .15f;
        private float curTime = .15f;
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerTag>();
            RequireForUpdate<PlayerMoveInput>();

            _playerInputActions = new PlayerInputActions();
        }
        protected override void OnStartRunning()
        {
            _playerInputActions.Enable();
            //_playerInputActions.Game.Shoot.performed += OnPlayerShoot;
            _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        }
        protected override void OnUpdate()
        {
            float2 curMoveInput = _playerInputActions.Game.Movement.ReadValue<Vector2>();
            SystemAPI.SetSingleton(new PlayerMoveInput { value = curMoveInput });
            if (curTime <= 0 && _playerInputActions.Game.Shoot.IsPressed())
            {
                if (!SystemAPI.Exists(_playerEntity)) return;
                SystemAPI.SetComponentEnabled<FireProjectileTag>(_playerEntity, true);
                curTime = fireRate;
            }
            else curTime -= SystemAPI.Time.DeltaTime;
        }
        protected override void OnStopRunning()
        {
            //_playerInputActions.Game.Shoot.performed -= OnPlayerShoot;
            _playerInputActions.Disable();
            _playerEntity = Entity.Null;
        }
        private void OnPlayerShoot(InputAction.CallbackContext obj)
        {
            if (!SystemAPI.Exists(_playerEntity)) return;
            SystemAPI.SetComponentEnabled<FireProjectileTag>(_playerEntity, true);
        }
    }
}

