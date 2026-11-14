namespace TestInfra.Screenplay;

public interface IQuestion<T>
{
    Task<T> AnsweredByAsync(Actor actor);
}
