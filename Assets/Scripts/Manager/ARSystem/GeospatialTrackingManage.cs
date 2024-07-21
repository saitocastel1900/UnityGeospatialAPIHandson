using AR_Fukuoka;
using Google.XR.ARCoreExtensions;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class GeospatialTrackingManage : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public IReactiveProperty<TrackingStatus.State> CurrentTrackingStateProp　=> _currentTrackingStateProp;
    private ReactiveProperty<TrackingStatus.State> _currentTrackingStateProp = new ReactiveProperty<TrackingStatus.State>(TrackingStatus.State.Initializing);

    /// <summary>
    /// 
    /// </summary>
    public IReactiveProperty<GeospatialPose> CurrentGeospatialPoseProp　=> _currentGeospatialPose;
    public GeospatialPose CurrentGeospatialPose => _currentGeospatialPose.Value;
    private ReactiveProperty<GeospatialPose> _currentGeospatialPose = new ReactiveProperty<GeospatialPose>();
    
    /// <summary>
    /// GeospatialAPIのトラッキング情報
    /// </summary>
    [SerializeField] private AREarthManager _earthManager;

    /// <summary>
    /// GeospatialAPIの初期化と結果
    /// </summary>
    [SerializeField] private VpsInitializer _vpsInitializer;
    
    /// <summary>
    /// 方位の許容精度
    /// </summary>
    [SerializeField] private double _headingThreshold = 25;

    /// <summary>
    /// 水平位置の許容精度
    /// </summary>
    [SerializeField] private double _horizontalThreshold = 20;

    private void Start()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_=>UpdateTrackingStatus())
            .AddTo(this.gameObject);
    }

    private void UpdateTrackingStatus()
    {
        if (!_vpsInitializer.IsReady || _earthManager.EarthTrackingState != TrackingState.Tracking)
        {
            _currentTrackingStateProp.SetValueAndForceNotify(TrackingStatus.State.Preparation);
            return;
        }
        
        GeospatialPose pose = _earthManager.CameraGeospatialPose;
        _currentGeospatialPose.SetValueAndForceNotify(pose);
        
        if (pose.OrientationYawAccuracy > _headingThreshold || pose.HorizontalAccuracy > _horizontalThreshold)
        {
            _currentTrackingStateProp.SetValueAndForceNotify(TrackingStatus.State.LowTrackingAccuracy);
        }
        else
        {
            _currentTrackingStateProp.SetValueAndForceNotify(TrackingStatus.State.HighTrackingAccuracy);
        }
    }
}
