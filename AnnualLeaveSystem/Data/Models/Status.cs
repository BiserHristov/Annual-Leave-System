namespace AnnualLeaveSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public enum Status
    {
        Approved = 0,
        Pending = 1,
        Canceled = 2,
        Rejected = 3
    }
}
