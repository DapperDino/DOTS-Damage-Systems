using DapperDino.DamageSystems.Components;
using Unity.Entities;
using Unity.Jobs;

namespace DapperDino.DamageSystems.Systems
{
    public class DeathCleanupSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

            Entities.WithoutBurst().WithAll<Dead>().ForEach((Entity entity) =>
            {
                ecb.DestroyEntity(entity);
            }).Run();

            return default;
        }
    }
}
