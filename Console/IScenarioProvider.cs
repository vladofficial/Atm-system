using System.Diagnostics.CodeAnalysis;

namespace Console;

public interface IScenarioProvider
{
    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario);
}