namespace Console;

public interface IScenario
{
    public string Name { get; }

    public void Run();
}