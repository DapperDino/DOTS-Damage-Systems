﻿using DapperDino.DamageSystems.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace DapperDino.DamageSystems.Systems
{
    [UpdateBefore(typeof(BlockSystem))]
    public class DamageCollisionSystem : JobComponentSystem
    {
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;

        protected override void OnCreate()
        {
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var damageCollisionJob = new DamageCollisionJob
            {
                damageGroup = GetBufferFromEntity<Damage>(),
                dealDamageGroup = GetComponentDataFromEntity<DealDamage>(true)
            };

            damageCollisionJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            return inputDeps;
        }

        private struct DamageCollisionJob : ITriggerEventsJob
        {
            public BufferFromEntity<Damage> damageGroup;
            [ReadOnly] public ComponentDataFromEntity<DealDamage> dealDamageGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (dealDamageGroup.HasComponent(triggerEvent.Entities.EntityA))
                {
                    if (damageGroup.Exists(triggerEvent.Entities.EntityB))
                    {
                        DealDamage dealDamage = dealDamageGroup[triggerEvent.Entities.EntityA];

                        damageGroup[triggerEvent.Entities.EntityB].Add(new Damage
                        {
                            DamageTypeId = dealDamage.DamageTypeId,
                            Value = dealDamage.Value
                        });
                    }
                }

                if (dealDamageGroup.HasComponent(triggerEvent.Entities.EntityB))
                {
                    if (damageGroup.Exists(triggerEvent.Entities.EntityA))
                    {
                        DealDamage dealDamage = dealDamageGroup[triggerEvent.Entities.EntityB];

                        damageGroup[triggerEvent.Entities.EntityA].Add(new Damage
                        {
                            DamageTypeId = dealDamage.DamageTypeId,
                            Value = dealDamage.Value
                        });
                    }
                }
            }
        }
    }
}
