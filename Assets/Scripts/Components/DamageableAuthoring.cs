using DapperDino.DamageSystems.Components;
using Unity.Entities;
using UnityEngine;

public class DamageableAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] private int startingHealth = 100;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<Damage>(entity);
        dstManager.AddComponentData(entity, new Health { Value = startingHealth });
    }
}
