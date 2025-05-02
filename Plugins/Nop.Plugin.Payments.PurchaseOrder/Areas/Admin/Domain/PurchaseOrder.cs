using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Infrastructure.Mapping;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain
{
    /// <summary>
    /// Represents a purchase order
    /// </summary>
    public class PurchaseOrder : BaseEntity
    {
        private ICollection<PurchaseOrderItem> _purchaseOrderItems;

        /// <summary>
        /// Gets or sets the supplier identifier
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the purchase order number
        /// </summary>
        public string PurchaseOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the purchase order status
        /// </summary>
        public int PurchaseOrderStatusId { get; set; }

        /// <summary>
        /// Gets or sets the customer (admin) who created the purchase order
        /// </summary>
        public int CreatedByCustomerId { get; set; }

        /// <summary>
        /// Gets or sets the total amount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the purchase order items
        /// </summary>
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems
        {
            get => _purchaseOrderItems ?? (_purchaseOrderItems = new List<PurchaseOrderItem>());
            protected set => _purchaseOrderItems = value;
        }
    }

}