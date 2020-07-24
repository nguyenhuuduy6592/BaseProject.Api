using System;

namespace BaseProject.Api.Data
{
    /// <summary>
    /// Add common Properties here that will be used for all your entities
    /// </summary>
    public class EntityBase
    {
        public long CreatedById { get; set; } = -1;
        public long ModifiedById { get; set; } = -1;
        public DateTime CreatedDateTimeUtc { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDateTimeUtc { get; set; } = DateTime.UtcNow;
    }
}
