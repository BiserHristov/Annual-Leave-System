namespace AnnualLeaveSystem.Test.Mocks
{
    using System;
    using AnnualLeaveSystem.Data;
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseMock
    {
        public static LeaveSystemDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<LeaveSystemDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new LeaveSystemDbContext(dbContextOptions);
            }
        }
    }
}
