using System;

public struct Place<T>
{
    public T Client { get; private set; }
    public bool IsFree { get; private set; }

    public void Reserve() =>
        IsFree = false;

    public void AddClient(T client)
    {
        if (Client != null)
            throw new Exception("Place is already occupied!");

        Client = client != null ? client : throw new ArgumentNullException(nameof(client));
    }

    public void Free()
    {
        IsFree = true;
        Client = default;
    }
}
