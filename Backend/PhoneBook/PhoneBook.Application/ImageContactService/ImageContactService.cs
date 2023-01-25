using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Models;
using PhoneBook.Infrastructure.IRepositories;

namespace PhoneBook.Application.ImageContactService
{
    public class ImageContactService : IContactImageService
    {
        private string basePath;
        private readonly ILogger<IContactImageService> logger;
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;
        private readonly IPhoneBookRepository<ImageContact> imageContactRepository;
        public ImageContactService(
            IHostingEnvironment environment,
            ILogger<IContactImageService> logger,
            IConfiguration configuration,
            IPhoneBookRepository<ImageContact> imageContactRepository)
        {
            this.environment = environment;
            this.logger = logger;
            this.configuration = configuration;
            this.imageContactRepository = imageContactRepository;

            initBasePath();
        }

        ///----------------------------------------
        ///     PRIVATE METHODS
        ///----------------------------------------

        private void initBasePath()
        {
#if DEBUG
            basePath = Path.Combine(environment.WebRootPath, configuration["Files:ImageContactPath"].ToString());
#else
            basePath = configuration["Files:ImageContactPath"].ToString();
#endif
        }

        /// <summary>
        /// Fayl manzili bo'yicha tekshiruv
        /// </summary>
        /// <param name="path">Fayl manzili</param>
        /// <param name="isCreate">Mavjud bo'lmasa, yangisini hosil qiladi.</param>
        /// <returns>Mavjud bo'lsa True : Aks holda Flase</returns>
        private bool existsCreatePath(string path, bool isCreate = false)
        {
            if (File.Exists(path))
                return true;
            if (isCreate)
            {
                try
                {
                    File.Create(path);
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError("ImageContactService existsCreatePath function " + ex.Message);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// prefix [Username]
        /// => Asos mazilga prefix qo'shib papkani tekshramiz agar bo'lmasa yaratamiz
        /// => File nomi ga GUID qo'shib, asosiy manzili bilan birlashtramiz va to'liq manzilni qaytramiz
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folder">Assosi manzilda yangi folderni ichiga file yo'lini yaratish</param>
        /// <returns></returns>
        private string getPath(string fileName, string folder = null)
        {
            // Username birlashtrlib yangi Papka yaratiladi
            string prefixPath = "zohidbek";

            if (folder is not null)
            {
                prefixPath = Path.Combine(folder, prefixPath);
            }

            // ikkita manzilni birlashtiramiz
            string pathString = Path.Combine(basePath, prefixPath);

            // manzilin tekshramiz va bo'lmasa yaratamiz
            if (!Directory.Exists(pathString))
            {
                // papkani yaratamiz
                Directory.CreateDirectory(pathString);
            }

            return Path.Combine(pathString, Guid.NewGuid().ToString() + "()" + fileName);
        }

        /// <summary>
        /// To'liq manzildan asosiy manzilni olib tashlaymiz
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string getBasePath(string path)
        {
            return path.Replace(basePath, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="filePath"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        private async Task<ImageContact> saveImageContact(IFormFile formFile, string filePath)
        {
            try
            {
                var model = new ImageContact()
                {
                    ContentPath = getBasePath(filePath),
                    FileName = formFile.FileName,
                    ContentLength = formFile.Length,
                    ContentType = formFile.ContentType,
                };
                return await imageContactRepository.AddAsync(model);
            }
            catch (Exception ex)
            {
                logger.LogError("ImageContactService => saveImageContact ", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// QA faylning sarlavhasini saqlaydi.
        /// </summary>
        /// <param name="formFile">Sarlavha file .jpg bo'lish shart</param>
        /// <param name="filePath">Sarlovha fileni diskdagi manzili</param>
        /// <param name="userProfile">Kim tamonidan yaratilmoqda</param>
        /// <returns></returns>
        private async Task<ImageContact> saveImageContactTemplateTitleFile(IFormFile formFile, string filePath)
        {
            try
            {
                // Agar QA filelar uchun oldin Sarlovha yaratgan bo'lsa eski Sarlovha yaroqsiz bo'ladi:
                var findFile = await imageContactRepository.GetFirstOrDefaultAsync(x => x.Active);

                // file qayta yuklanmasligi uchun nomi va hajmi bilan solishtramiz
                if (findFile.FileName == formFile.FileName && findFile.ContentLength == formFile.Length)
                    return null;

                if (findFile is not null)
                {
                    findFile.Active = false;
                    await imageContactRepository.UpdateAsync(findFile);
                }

                var model = new ImageContact()
                {
                    FileName = formFile.FileName,
                    ContentLength = formFile.Length,
                    ContentType = formFile.ContentType,
                    ContentPath = getBasePath(filePath)
                };
                return await imageContactRepository.AddAsync(model);
            }
            catch (Exception ex)
            {
                logger.LogError("ImageContactService => saveImageContactTemplateTitleFile ", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// File manzili beriladi
        /// versiya nomeriga qarab coopy yasaladi
        /// D:\test.docx => D:\test+versionNumber+.docx
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="versionNumber"></param>
        /// <returns></returns>
        private Tuple<bool, string> CopyFileByPath(string filePath, string copyFileName = "")
        {
            try
            {
                // Manzilni birlashtiramiz

                filePath = Path.Combine(basePath, filePath);
                copyFileName = Path.Combine(basePath, copyFileName);
                if (!File.Exists(filePath)) return new Tuple<bool, string>(false, "Fayl topilmadi.");
                else
                {
                    string newFilePath = copyFileName == "" ? filePath + Guid.NewGuid().ToString() : copyFileName;
                    // newFilePath katalog ichida bo'lish kerak emas
                    if (!File.Exists(newFilePath))
                    {
                        File.Copy(filePath, newFilePath, true);
                        return new(true, newFilePath);
                    }
                    else return new(false, "File katalog ichida mavjud. :)");
                }
            }
            catch (Exception ex)
            {
                return new(false, ex.Message);
            }
        }

        /// <summary>
        /// DTO object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private ImageContactDTO GetModelDTO(ImageContact model)
        {
            return new ImageContactDTO()
            {
                FileName = model.FileName,
                ContentLength = model.ContentLength,
                ContentPath = model.ContentPath,
                ContentType = model.ContentType,
            };
        }


        ///----------------------------------------
        ///     PUBLIC GET METHODS
        ///----------------------------------------

        public string GetPath(string path)
        {
            return Path.Combine(basePath, path);
        }

        /// <summary>
        /// Get IFromFile by identity key
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>File topilsa FormFile, aks holda null</returns>
        public async Task<IFormFile> GetFileById(Guid fileId)
        {
            var file = await imageContactRepository.FindAsync(fileId);

            if (file is null) return null;

            string path = Path.Combine(basePath, file.ContentPath);


            if (file == null || !existsCreatePath(path))
                return null;

            using var stream = File.OpenRead(path);

            return new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = file.ContentType
            };
        }

        /// <summary>
        /// FileId bo'yicha jadvaldagi modelni olish
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>=> null id bo'yicha jadvalda mavjud emas: Aks holda model</returns>
        public async Task<ImageContactDTO> GetImageContactById(Guid fileId)
        {
            var findFile = await imageContactRepository.FindAsync(fileId);
            if (findFile == null) return null;

            return GetModelDTO(findFile);
        }

        /// <summary>
        /// Get IFromFile by file source path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>File topilsa FormFile, aks holda null</returns>
        public async Task<IFormFile> GetFileByPath(string path)
        {
            path = Path.Combine(basePath, path);
            if (!existsCreatePath(path))
                return null;

            using var stream = File.OpenRead(path);
            string contentType = "";
            new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
            return new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        /// <summary>
        /// Get stirng base64 encode file by identity key
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>stringbase64 | null</returns>
        public async Task<string> GetBase64ById(Guid fileId, string prefix = "")
        {
            var qaFile = await imageContactRepository.FindAsync(fileId);

            var filesPath = Directory.GetCurrentDirectory();

            string physicalPath = "wwwroot/zohidbek" + qaFile.ContentPath;

            var imagePath = Path.Combine(filesPath, physicalPath);


            if (!File.Exists(imagePath))
            {
                return String.Empty;
            }

            var bytes = await File.ReadAllBytesAsync(imagePath);

            return qaFile.ContentType == "data:image/png;base64," ? "data:image/png;base64," + Convert.ToBase64String(bytes) : "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Get string base64 encode file by  source path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>stringbase64 | null</returns>
        public async Task<string> GetBase64ByPath(string path, string prefix = "")
        {
            path = Path.Combine(basePath, path);
            if (!existsCreatePath(path))
                return null;

            var bytes = await File.ReadAllBytesAsync(path);
            return prefix + Convert.ToBase64String(bytes);
        }


        ///----------------------------------------
        ///     PUBLIC POST METHODS
        ///----------------------------------------

        /// <summary>
        /// IFromFile
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userProfile"></param>
        /// <returns> =>0 no saved file or saved file id from DB</returns>
        public async Task<Guid> UploadFileToFolder(IFormFile file)
        {
            if (file == null) return Guid.Empty;

            // file manzilni olish
            string uploadPath = getPath(file.FileName);

            // file ma'lumotlarni DB TbOrderFile jadvaliga saqlandimi.
            var isSaveFile = await saveImageContact(file, uploadPath);
            if (isSaveFile is null)
                return Guid.Empty;

            using Stream stream = new FileStream(uploadPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return isSaveFile.Id;
        }

        /// <summary>
        /// IFormFile va model birgalikda  diskga va jadvalga yozamiz
        /// </summary>
        /// <param name="file">IFormFile</param>
        /// <param name="model">Model</param>
        /// <param name="userProfile">Faoydalanuvchi</param>
        /// <returns></returns>
        public async Task<Guid> onSaveImageContactWithUploadToFolder(IFormFile file, ImageContactDTO model)
        {
            if (file is null || model is null) return Guid.Empty;

            // file manzilni olish
            string uploadPath = getPath(file.FileName);

            var modelDb = new ImageContact()
            {
                ContentPath = getBasePath(uploadPath),
                ContentLength = model.ContentLength,
                ContentType = model.ContentType,
                FileName = model.FileName,
            };
            var isSaved = await imageContactRepository.AddAsync(modelDb);

            if (isSaved is null) return Guid.Empty;

            using Stream stream = new FileStream(uploadPath, FileMode.Create);
            await stream.CopyToAsync(stream);

            return isSaved.Id;
        }

        /// <summary>
        /// File Id bo'yicha copya olamiz va yangi file nom beramiz
        /// 
        /// </summary>
        /// <param name="fileId">Qaysi file Copya qilinadi</param>
        /// <param name="endPrefix">Copya qilinyotgan fileni nomiga qo'shimcha prefix biriktiradi va yanig fileni nomi bo'ladi</param>
        /// <param name="userProfile">Kim tamonidan copya qilinyabdi </param>
        /// <returns></returns>
        public async Task<Guid> DeepCopyFileById(Guid fileId, string endPrefix)
        {
            var findFile = await imageContactRepository.FindAsync(fileId);
            if (findFile is null) return Guid.Empty;

            string copyFileName = findFile.ContentPath + endPrefix;

            var copyFile = CopyFileByPath(findFile.ContentPath, copyFileName);
            if (copyFile.Item1 == false) return Guid.Empty;
            else
            {
                ImageContact file = new ImageContact();
                file.Id = Guid.Empty;
                file.ContentType = findFile.ContentType;
                file.CreatedDate = findFile.CreatedDate;
                file.ContentLength = findFile.ContentLength;
                file.ContentPath = copyFile.Item2;
                await imageContactRepository.AddAsync(file);
                return file.Id;
            }
        }
    }
}
