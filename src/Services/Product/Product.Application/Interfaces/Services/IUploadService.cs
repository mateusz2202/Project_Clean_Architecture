using Product.Application.Requests;

namespace Product.Application.Interfaces.Services;

public interface IUploadService
{
    string UploadAsync(UploadRequest request);
}
