public interface IBusReceiver
{
    int GetFreeStopIndex();
    void AddBusOnWayToStop(Bus bus, int spotIndex);
    void TakeBus(Bus bus, int platformIndex);
    void ReleaseStop(int index);
}
