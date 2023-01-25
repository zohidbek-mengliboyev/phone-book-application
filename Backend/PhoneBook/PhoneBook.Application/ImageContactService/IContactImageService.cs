using Microsoft.AspNetCore.Http;
using PhoneBook.Domain.Models;

namespace PhoneBook.Application.ImageContactService
{
    public interface IContactImageService
    {
        string GetPath(string path);
        //----------- GET -----------------//

        /// <summary>
        /// Get IFromFile by identity key
        /// </summary>
        /// <param name="fileId">fileId identity key</param>
        /// <returns></returns>
        Task<IFormFile> GetFileById(Guid fileId);

        /// <summary>
        /// FileId bo'yicha jadvaldagi modelni olish
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>=> null id bo'yicha jadvalda mavjud emas: Aks holda model</returns>
        Task<ImageContactDTO> GetImageContactById(Guid fileId);

        /// <summary>
        /// Get IFromFile by file source path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<IFormFile> GetFileByPath(string path);

        /// <summary>
        /// Get stirng base64 encode file by identity key
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<string> GetBase64ById(Guid fileId, string prefix = "");

        /// <summary>
        /// Get string base64 encode file by  source path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<string> GetBase64ByPath(string path, string prefix = "");

        //------------------- POST ---------------//

        /// <summary>
        /// Upload the given file to the root path
        /// Berilgan file asosiy yo'lga yuklash
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        Task<Guid> UploadFileToFolder(IFormFile file);

        /// <summary>
        /// Shakilangan modelni DBga yozish
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        Task<Guid> onSaveImageContactWithUploadToFolder(IFormFile file, ImageContactDTO model);

        /// <summary>
        /// Deep copy file
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="copyFileName"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        Task<Guid> DeepCopyFileById(Guid fileId, string copyFileName);
    }
}
