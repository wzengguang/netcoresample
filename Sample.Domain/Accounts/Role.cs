using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Domain.Entities
{
    public class Role : IdentityRole<long>, IEntityBase, IEntitySoftDeletedAndCreated
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedTime { get; set; } = DateTime.Now;

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime UpdatedTime { get; set; }

        public long? CreatedUserId { get; set; }
    }
}
 