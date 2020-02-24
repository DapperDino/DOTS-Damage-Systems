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
                for (int i = damageBuffer.Length - 1; i >= 0; i--)
                {
                    int damageToBlock = math.min(block.Value, damageBuffer[i].Value);
                    block.Value -= damageToBlock;

                    damageBuffer.Insert(i, new Damage
                    {
                        DamageTypeId = damageBuffer[i].DamageTypeId,
                        Value = damageBuffer[i].Value - damageToBlock
                    });
                    damageBuffer.RemoveAt(i + 1);

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