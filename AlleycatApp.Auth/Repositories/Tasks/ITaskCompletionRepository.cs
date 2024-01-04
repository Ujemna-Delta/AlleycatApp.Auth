using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Tasks
{
    public interface ITaskCompletionRepository : ICrudRepository<TaskCompletion, int>
    {
        Task<IEnumerable<TaskCompletion>> GetByUserIdAsync(string userId);
        Task<IEnumerable<TaskCompletion>> GetByTaskIdAsync(int taskId);
    }
}
