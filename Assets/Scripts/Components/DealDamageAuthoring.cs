using Unity.Entities;
using UnityEngine;

namespace DapperDino.DamageSystems.Components
{
    public struct DealDamage : IComponentData
    {
        public int DamageTypeId;
        public int Value;
    }
    public class DealDamageAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private DamageType damageType = null;
        [SerializeField] private int value = 50;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new DealDamage { DamageTypeId = damageType.Id, Value = value });
        }
    }
}
