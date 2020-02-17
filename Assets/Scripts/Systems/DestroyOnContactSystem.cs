using DapperDino.DamageSystems.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace DapperDino.DamageSystems.Systems
{
    public class DestroyOnContactSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var destroyOnContactGroup = GetComponentDataFromEntity<DestoryOnContact>(true);
            var ecb = ecbSystem.CreateCommandBuffer();

            var destroyTriggerJob = new DestroyTriggerJob
            {
                ecb = ecb,
                destroyOnContactGroup = destroyOnContactGroup
            };

            var destroyCollisionJob = new DestroyCollisionJob
            {
                ecb = ecb,
                destroyOnContactGroup = destroyOnContactGroup
            };

            destroyTriggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();
            destroyCollisionJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps).Complete();

            return inputDeps;
        }

        private struct DestroyTriggerJob : ITriggerEventsJob
        {
            public EntityCommandBuffer ecb;
            [ReadOnly] public ComponentDataFromEntity<DestoryOnContact> destroyOnContactGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (destroyOnContactGroup.HasComponent(triggerEvent.Entities.EntityA))
                {
                    ecb.DestroyEntity(triggerEvent.Entities.EntityA);
                }

                if (destroyOnContactGroup.HasComponent(triggerEvent.Entities.EntityB))
                {
                    ecb.DestroyEntity(triggerEvent.Entities.EntityB);
                }
            }
        }

        private struct DestroyCollisionJob : ICollisionEventsJob
        {
            public EntityCommandBuffer ecb;
            [ReadOnly] public ComponentDataFromEntity<DestoryOnContact> destroyOnContactGroup;

            public void Execute(CollisionEvent collisionEvent)
            {
                if (destroyOnContactGroup.HasComponent(collisionEvent.Entities.EntityA))
                {
                    ecb.DestroyEntity(collisionEvent.Entities.EntityA);
                }

                if (destroyOnContactGroup.HasComponent(collisionEvent.Entities.EntityB))
                {
                    ecb.DestroyEntity(collisionEvent.Entities.EntityB);
                }
            }
        }
    }
}
