public static class TrackingStatus
{
    public enum State
    {
        None,
        Initializing,
        Preparation,
        LowTrackingAccuracy,
        HighTrackingAccuracy
    }
}