namespace AnnualLeaveSystem.Services.LeaveTypes
{
    using AnnualLeaveSystem.Data;
    using System.Linq;

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
