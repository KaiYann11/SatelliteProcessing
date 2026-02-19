// 파일: SatelliteProcessing.Infrastructure/Storage/JobStoreModel.cs
// 설명: 작업 정보를 파일에 저장하기 위한 직렬화 모델입니다.

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Defines the persisted payload for job storage.
/// </summary>
public sealed class JobStoreModel
{
    /// <summary>
    /// Gets or sets the job records stored in the file.
    /// </summary>
    public List<JobRecord> Jobs { get; set; } = new();
}
