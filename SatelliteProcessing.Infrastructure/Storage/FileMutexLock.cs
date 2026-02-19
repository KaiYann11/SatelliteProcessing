// 파일: SatelliteProcessing.Infrastructure/Storage/FileMutexLock.cs
// 설명: 파일 기반 뮤텍스(lock) 구현입니다. 파일 잠금으로 동시성 제어를 수행합니다.

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Provides a named mutex to coordinate access to shared files across processes.
/// </summary>
public sealed class FileMutexLock : IDisposable
{
    private readonly Mutex _mutex;
    private bool _hasLock;

    /// <summary>
    /// Initializes a new lock with the provided mutex name.
    /// </summary>
    /// <param name="mutexName">The system-wide mutex name.</param>
    public FileMutexLock(string mutexName)
    {
        _mutex = new Mutex(false, mutexName);
    }

    /// <summary>
    /// Waits until the lock is acquired or the operation is canceled.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the wait operation.</param>
    public void Wait(CancellationToken cancellationToken)
    {
        while (!_hasLock)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Use a short timeout so cancellation can be observed regularly.
            _hasLock = _mutex.WaitOne(TimeSpan.FromMilliseconds(200));
        }
    }

    /// <summary>
    /// Releases the lock if it was acquired.
    /// </summary>
    public void Dispose()
    {
        if (_hasLock)
        {
            _mutex.ReleaseMutex();
        }

        _mutex.Dispose();
    }
}
