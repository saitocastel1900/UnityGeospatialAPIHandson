using System;
using Zenject;

public class InputEventInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(IInputEventProvider), 
                typeof(IInitializable), typeof(IDisposable))
            .To<TouchInputProvider>().AsSingle();
    }
}