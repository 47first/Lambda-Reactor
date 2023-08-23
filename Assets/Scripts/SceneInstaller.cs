using UnityEngine;
using Zenject;

namespace Runtime
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private EnvironmentController _environmentController;
        public override void InstallBindings()
        {
            Container.BindInstance(_environmentController);
        }
    }
}
