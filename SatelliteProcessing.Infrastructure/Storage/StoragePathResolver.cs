// 파일: SatelliteProcessing.Infrastructure/Storage/StoragePathResolver.cs
// 설명: 저장소 데이터 디렉터리 및 경로를 결정하는 유틸리티입니다.

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Resolves configured storage paths to absolute paths.
/// </summary>
public static class StoragePathResolver
{
    /// <summary>
    /// Resolves the storage directory using the provided base directory.
    /// </summary>
    /// <param name="configuredPath">The configured directory path.</param>
    /// <param name="baseDirectory">The base directory used for relative paths.</param>
    /// <returns>An absolute path to the storage directory.</returns>
    public static string ResolveDataDirectory(string configuredPath, string baseDirectory)
    {
        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            return baseDirectory;
        }

        if (Path.IsPathRooted(configuredPath))
        {
            return configuredPath;
        }

        return Path.GetFullPath(Path.Combine(baseDirectory, configuredPath));
    }
}
