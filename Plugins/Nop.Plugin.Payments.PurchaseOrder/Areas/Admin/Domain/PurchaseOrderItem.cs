using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;
/// <summary>
/// Represents a purchase order item
/// </summary>
public class PurchaseOrderItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the purchase order identifier
    /// </summary>
    public int PurchaseOrderId { get; set; }

    /// <summary>
    /// Gets or sets the product identifier
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit cost
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// Gets or sets the total cost
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Gets or sets the purchase order
    /// </summary>
    public virtual PurchaseOrder PurchaseOrder { get; set; }
}

/// <summary>
/// Purchase order status enum
/// </summary>
public enum PurchaseOrderStatus
{
    /// <summary>
    /// Created
    /// </summary>
    Created = 10,

    /// <summary>
    /// Received
    /// </summary>
    Received = 20,

    /// <summary>
    /// Cancelled
    /// </summary>
    Cancelled = 30
}

