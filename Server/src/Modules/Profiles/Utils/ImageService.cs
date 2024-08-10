namespace BlogBackend.Modules.Profiles.Utils;

public interface IImageService {
    Task<string> Upload(IFormFile file, CancellationToken cancellationToken);
    void Delete(ProfileImage image);
};
public class ImageService(IWebHostEnvironment env) : IImageService
{
    private readonly string _uploadFolder
        = Path.Combine(env.WebRootPath, "images");
    public async Task<string> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_uploadFolder))
        {
            Directory.CreateDirectory(_uploadFolder);
        }
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_uploadFolder, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {

            await file.CopyToAsync(fileStream, cancellationToken);
        }
        return  $"api/images/{fileName}";
    }

    public void Delete(ProfileImage image)
    {
        var oldFileName = Path.GetFileName(image.Value);
        var oldImage = Path.Combine(_uploadFolder,oldFileName);
        if (File.Exists(oldImage))
        {
            File.Delete(oldImage);
        }
    }
}

