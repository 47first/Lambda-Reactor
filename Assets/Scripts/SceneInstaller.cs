using System.Collections;
using UnityEngine;
using Zenject;

namespace Runtime
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private EnvironmentController _environmentController;
        public override void InstallBindings()
        {
            Container.Bind<IEnvironmentController>().FromInstance(_environmentController);
            Container.Bind<QueryController>().AsSingle().NonLazy();
        }

        private IEnumerator Start()
        {
            yield return null;
            Container.Resolve<QueryController>().Next();
        }
    }
}
