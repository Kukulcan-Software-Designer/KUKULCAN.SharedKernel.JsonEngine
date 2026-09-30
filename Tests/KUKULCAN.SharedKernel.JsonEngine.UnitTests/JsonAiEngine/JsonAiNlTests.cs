using KUKULCAN.SharedKernel.JsonEngine.AI;

namespace KUKULCAN.SharedKernel.JsonEngine.UnitTests.JsonAiEngine;

public class JsonAiNlTests
{
    [Test]
    public void ToAiQuery_ShouldGeneratePipeline()
    {
        const string nl = "Muéstrame los items caros y sus dependencias ordenadas por importancia";
        var ai = JsonAiNl.ToAiQuery(nl);

        Assert.That(ai, Does.Contain("FIND items WHERE precio.valor > 50"));
        Assert.That(ai, Does.Contain("GRAPH EXPAND"));
        Assert.That(ai, Does.Contain("RANK BY PAGERANK"));
        Assert.That(ai, Does.Contain("RETURN id, nombre"));
    }
}
