using DapperDino.DamageSystems.Components;
using Unity.Entities;
using Unity.Jobs;

namespace DapperDino.DamageSystems.Systems
{
    [UpdateBefore(typeof(BlockSystem))]
    public class DamageResistanceSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Entities.WithoutBurst().ForEach((ref DynamicBuffer<Damage> damageBuffer, in DynamicBuffer<DamageResistance> resistances) =>
            {
                for (int i = damageBuffer.Length - 1; i >= 0; i--)
                {
                    for (int j = 0; j < resistances.Length; j++)
                    {
                        if (damageBuffer[i].DamageTypeId == resistances[j].DamageTypeId)
                        {
                            damageBuffer.Insert(i, new Damage
                            {
                                DamageTypeId = damageBuffer[i].DamageTypeId,
                                Value = damageBuffer[i].Value * (1 / resistances[j].Value)
                            });
                            damageBuffer.RemoveAt(i + 1);

                            break;
                        }
                    }
                }
            }).Run();

            return default;
        }
    }
}
