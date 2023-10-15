using Identity.Application.Requests;

namespace Identity.Application.Common.Interfaces;

public interface IUploadService
{
    string UploadAsync(UploadRequest request);
}
