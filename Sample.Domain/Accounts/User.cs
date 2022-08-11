using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Domain.Accounts
{
    public class User : IdentityUser<long>, IEntitySoftDeletedAndCreated
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [MaxLength(256)]
        public string WeAppOpenId { get; set; }

        [MaxLength(256)]
        public string WeAppUOpenId { get; set; }

        [MaxLength(256)]
        public string WeAppSessionKey { get; set; }

        [MaxLength(256)]
        public string WeAppNickName { get; set; }

        /// <summary>
        /// Resource.ServerFilePath
        /// </summary>
        [MaxLength(256)]
        public string AvatarFilePath { get; set; }

        [MaxLength(256)]
        public string NickName { get; set; }

        public int Gender { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(32)]
        public string Directory { get; set; }

        public DateTime DeletedTime { get; set; } = DateTime.Now;

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime UpdatedTime { get; set; }

        public long? CreatedUserId { get; set; }
    }
}
