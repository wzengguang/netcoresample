using System.ComponentModel.DataAnnotations;

namespace Sample.Domain.Resources
{
    public class Resource : EntitySoftDeletedAndCreated
    {
        [MaxLength(64)]
        public string ServerFileName { get; set; }

        [MaxLength(128)]
        public string ServerFilePath { get; set; }

        [MaxLength(128)]
        public string UploadFileName { get; set; }

        [MaxLength(128)]
        public string MD5 { get; set; }

        [MaxLength(32)]
        public string FileExtension { get; set; }

        public long? RecipeId { get; set; }
    }
}
