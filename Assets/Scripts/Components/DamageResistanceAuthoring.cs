using Unity.Entities;
using UnityEngine;

namespace DapperDino.DamageSystems.Components
{
    [InternalBufferCapacity(2)]
    public struct DamageResistance : IBufferElementData
    {
        public int DamageTypeId;
        public float Value;
    }

    public class DamageResistanceAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private DamageResistanceStats damageResistanceStats = null;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var buffer = dstManager.AddBuffer<DamageResistance>(entity);

            foreach (var resistance in damageResistanceStats.Resistances)
            {
                buffer.Add(new DamageResistance { DamageTypeId = resistance.DamageType.Id, Value = resistance.Value });
            }
        }
    }
}
