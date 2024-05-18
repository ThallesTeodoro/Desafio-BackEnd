namespace DesafioBackEnd.Domain.Contracts.Services;

public interface IStorageService
{
    /// <summary>
    /// Upload a file to storage service
    /// </summary>
    /// <param name="file"></param>
    /// <param name="prefix"></param>
    /// <param name="fileExtension"></param>
    /// <param name="fileContentType"></param>
    /// <returns>File name</returns>
    Task<string?> UploadFileAsync(Stream file, string prefix, string fileExtension, string fileContentType);

    /// <summary>
    /// Delete file in storage if exists
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Task</returns>
    Task DeleteFileIfExistsAsync(string fileName, CancellationToken cancellationToken = default);
}
