public interface IBusReceiver
{
    int GetFreeStopIndex();
    void TakeBus(Bus bus, int platformIndex);
    void ReleaseStop(int index);
}
