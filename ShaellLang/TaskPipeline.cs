using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShaellLang;

public class TaskPipeline 
{
    List<Task> _scheduledTasks = new List<Task>();

    public void Execute()
    {
        foreach (var task in _scheduledTasks)
        {
            task.Wait();
        }
    }
    
    private void Schedule(Task task)
    {
        _scheduledTasks.Add(task);
    }
    
    
}