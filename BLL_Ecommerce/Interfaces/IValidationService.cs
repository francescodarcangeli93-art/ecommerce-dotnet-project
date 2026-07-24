

namespace BLL_Ecommerce.Interfaces
{
    public interface IValidationService
    {
        ValidationResult ValidateImage(byte[] fileBytes, string fileName, int maxKb = 2048, string[] allowedExtensions = null);
        ValidationResult ValidateDocument(byte[] fileBytes, string fileName, int maxKb = 20000, string[] allowedExtensions = null);
    }
}
