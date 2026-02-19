using System.Security.Cryptography;
using System.Text;

// 파일: SatelliteProcessing.Infrastructure/Storage/MutexNameBuilder.cs
// 설명: 파일 기반 뮤텍스 이름을 생성하기 위한 유틸리티입니다. 파일 잠금 식별자 생성에 사용됩니다.

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Builds stable mutex names based on a file path.
/// </summary>
public static class MutexNameBuilder
{
    /// <summary>
    /// Creates a mutex name using the provided prefix and file path.
    /// </summary>
    /// <param name="prefix">Prefix to distinguish mutex scopes.</param>
    /// <param name="filePath">File path to hash for uniqueness.</param>
    /// <returns>A stable mutex name safe for cross-process use.</returns>
    public static string Create(string prefix, string filePath)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(filePath);
        var hash = sha256.ComputeHash(bytes);
        var hashText = Convert.ToHexString(hash);

        return $"Local\\{prefix}.{hashText}";
    }
}
