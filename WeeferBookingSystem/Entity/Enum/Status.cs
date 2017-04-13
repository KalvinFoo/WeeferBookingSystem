using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeeferBookingSystem.Entity
{
    public enum Status : int
    {
        Draft = 0,
        PendingForApproval = 1,
        Approved = 2,
        Cancelled = 3,
    }
}