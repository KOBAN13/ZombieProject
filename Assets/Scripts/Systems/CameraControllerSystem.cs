using Aspects;
using ComponentsAndTags.Brain;
using DefaultNamespace;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    public partial class CameraControllerSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<BrainProperties>();
        }

        protected override void OnUpdate()
        {
            var camera = CameraControllerSingleton.instance;
            var scale = SystemAPI.GetAspect<BrainAspect>(SystemAPI.GetSingletonEntity<BrainProperties>());

            var positionFactor = (float)SystemAPI.Time.ElapsedTime * camera.Speed;
            var height = camera.HeightAtScale(scale.Scale);
            var radius = camera.RadiusAtScale(scale.Scale);

            camera.transform.position = new Vector3(Mathf.Cos(positionFactor) * radius, height,
                Mathf.Sin(positionFactor) * radius);
            
            camera.transform.LookAt(Vector3.zero, Vector3.up);
        }
    }
}