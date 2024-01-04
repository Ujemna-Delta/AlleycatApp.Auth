using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Tasks
{
    public interface ITaskRepository : ICrudRepository<TaskModel, int>
    {
        Task<IEnumerable<TaskModel>> GetByPointId(int pointId);
    }
}
