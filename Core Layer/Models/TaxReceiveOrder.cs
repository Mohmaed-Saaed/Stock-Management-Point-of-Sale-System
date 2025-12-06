using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class TaxReceiveOrder
    {
        public int TaxId { get; set; }
        public Tax Tax { get; set; } = null!;
        public int OperationId { get; set; }
        public ReceiveOrder ReceiveOrder { get; set; } = null!;
    }
}
