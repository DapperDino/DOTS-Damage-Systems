using DapperDino.DamageSystems.Components;
using Unity.Entities;
using Unity.Jobs;

namespace DapperDino.DamageSystems.Systems
{
    [UpdateBefore(typeof(DeathCleanupSystem))]
    public class ResolveDamageSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

            Entities.WithoutBurst().WithNone<Dead>().ForEach((Entity entity, ref DynamicBuffer<Damage> damageBuffer, ref Health health) =>
            {
                foreach (var damage in damageBuffer)
                {
                    health.Value -= damage.Value;

                    if (health.Value <= 0)
                    {
                        health.Value = 0;
                        ecb.AddComponent<Dead>(entity);
                        break;
                    }
                }

                damageBuffer.Clear();
            }).Run();

            return default;
        }
    }
}
