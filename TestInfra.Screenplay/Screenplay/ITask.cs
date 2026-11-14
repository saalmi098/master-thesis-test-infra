namespace TestInfra.Screenplay;

public interface ITask
{
    Task PerformAsAsync(Actor actor);
}
