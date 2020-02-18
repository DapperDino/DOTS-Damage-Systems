using DapperDino.DamageSystems.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace DapperDino.DamageSystems.Systems
{
    [UpdateBefore(typeof(ResolveDamageSystem))]
    public class BlockSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

            Entities.WithoutBurst().ForEach((Entity entity, ref Block block, ref DynamicBuffer<Damage> damageBuffer) =>
            {
                if (damageBuffer.Length == 0) { return; }

                DynamicBuffer<int> damageValueBuffer = damageBuffer.Reinterpret<int>();

                for (int i = 0; i < damageValueBuffer.Length; i++)
                {
                    int damageToBlock = math.min(block.Value, damageValueBuffer[i]);
                    block.Value -= damageToBlock;
                    damageValueBuffer[i] -= damageToBlock;

                    if (block.Value == 0)
                    {
                        ecb.RemoveComponent<Block>(entity);
                    }
                }
            }).Run();

            return default;
        }
    }
}