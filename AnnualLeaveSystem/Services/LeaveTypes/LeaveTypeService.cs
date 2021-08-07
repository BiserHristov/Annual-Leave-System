namespace AnnualLeaveSystem.Services.LeaveTypes
{
    using System.Linq;
    using AnnualLeaveSystem.Data;

    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly LeaveSystemDbContext db;

        public LeaveTypeService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public bool TypeExist(int id)
        {
            return this.db.LeaveTypes.Any(lt => lt.Id == id);
        }
    }
}
