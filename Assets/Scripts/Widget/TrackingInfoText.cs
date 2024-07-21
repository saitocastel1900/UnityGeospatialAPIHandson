using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class TrackingInfoText : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private GeospatialTrackingManage _geospatialTrackingManage;
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Text _text;

    private void Start()
    {
        _geospatialTrackingManage
            .CurrentTrackingStateProp
            .Subscribe(SetTrackingInfo)
            .AddTo(this.gameObject);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    private void SetTrackingInfo(TrackingStatus.State state)
    {
        string status = state switch
        {
            TrackingStatus.State.Initializing => "初期化中",
            TrackingStatus.State.Preparation => "準備中",
            TrackingStatus.State.LowTrackingAccuracy => "低精度：周辺を見回してください",
            TrackingStatus.State.HighTrackingAccuracy => "高精度：High Tracking Accuracy",
            _ => "例外が発生しました",
        };

        var pose = _geospatialTrackingManage.CurrentGeospatialPose;
        _text.text = string.Format(
            "緯度/経度: {0}°, {1}°\n" +
            "水平精度の精度: {2}m\n" +
            "高度: {3}m\n" +
            "高度の精度: {4}m\n" +
            "方位: {5}°\n" +
            "方位の精度: {6}°\n" +
            "{7} \n"
            ,
            pose.Latitude.ToString("F6"),
            pose.Longitude.ToString("F6"),
            pose.HorizontalAccuracy.ToString("F6"),
            pose.Altitude.ToString("F2"),
            pose.VerticalAccuracy.ToString("F2"),
            pose.EunRotation.ToString("F1"), //{5}
            pose.OrientationYawAccuracy.ToString("F1"),
            status
        );
    }
}