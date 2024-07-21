using System;
using UniRx;
using UnityEngine;
using Zenject;

public class TouchInputProvider : IInputEventProvider,IInitializable,IDisposable
{
    IReadOnlyReactiveProperty<bool> IInputEventProvider.InputTapRelease => _inputTapRelease;
    private BoolReactiveProperty _inputTapRelease=new BoolReactiveProperty();
    
    IReadOnlyReactiveProperty<bool> IInputEventProvider.InputTapPush => _inputTapPush;
    private BoolReactiveProperty _inputTapPush=new BoolReactiveProperty();

    public IReadOnlyReactiveProperty<Vector3> InputTapPosition => _inputTapPosition;
    private Vector3ReactiveProperty _inputTapPosition=new Vector3ReactiveProperty();
    
    /// <summary>
    /// 
    /// </summary>
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    
    public void Initialize()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            .Select(_ =>true)
            .Subscribe(_inputTapRelease.SetValueAndForceNotify).AddTo(_compositeDisposable);

        Observable.EveryUpdate()
            .Where(_ =>  Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            .Select(_ =>true)
            .Subscribe(_inputTapPush.SetValueAndForceNotify).AddTo(_compositeDisposable);

        _inputTapPush
            .Where(_ => Input.touchCount > 0)
            .Select(_ =>(Vector3)(Input.GetTouch(0).position))
            .Subscribe(_inputTapPosition.SetValueAndForceNotify).AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}
