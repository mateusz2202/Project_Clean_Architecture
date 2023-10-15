using Document.Application.Requests;

namespace Document.Application.Interfaces.Services;

public interface IUploadService
{
    string UploadAsync(UploadRequest request);
}
