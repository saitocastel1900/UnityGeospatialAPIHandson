using UniRx;
using UnityEngine;

public interface IInputEventProvider
{
    /// 
    /// </summary>
    /// <returns></returns>
    IReadOnlyReactiveProperty<bool> InputTapRelease { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IReadOnlyReactiveProperty<bool> InputTapPush{ get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IReadOnlyReactiveProperty<Vector3> InputTapPosition { get; }
}
