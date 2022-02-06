using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    public class PersistedGrant
    {
        [Key]
        public string Id { get; set; }
        public string Type { get; set; }
        public string SubjectId { get; set; }
        public string SessionId { get; set; }
        public string ClientId { get; set; }
        public string Description { get; set; }
        public string CreationTime { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public DateTimeOffset ConsumedTime { get; set; }
        public string Data { get; set; }
    }
}
