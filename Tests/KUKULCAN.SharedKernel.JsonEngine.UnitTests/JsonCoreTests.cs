using System.Text.Json.Nodes;

namespace KUKULCAN.SharedKernel.JsonEngine.UnitTests;

public class JsonCoreTests
{
    private readonly JsonNode _root;

    public JsonCoreTests()
    {
        var json = """
                   {
                     "item": {
                       "id": "A",
                       "precio": { "valor": 120 },
                       "tags": ["uno", "dos"]
                     }
                   }
                   """;

        _root = JsonCore.Parse(json)!;
    }

    [Test]
    public void Parse_ShouldLoadJson()
    {
        Assert.That(_root, Is.Not.Null);
        Assert.That(JsonCore.GetString(_root, "item.id"), Is.EqualTo("A"));
    }

    [Test]
    public void JsonPath_ShouldNavigateCorrectly()
    {
        var node = JsonCore.JsonPath(_root, "$.item.precio.valor");
        Assert.That(node!.ToString(), Is.EqualTo("120"));
    }

    [Test]
    public void GetDouble_ShouldReturnNumericValue()
    {
        var val = JsonCore.GetDouble(_root, "item.precio.valor");
        Assert.That(val, Is.EqualTo(120));
    }

    [Test]
    public void EvalCondition_ShouldEvaluateNumericExpression()
    {
        var ok = JsonCore.EvalCondition(_root["item"]!, "precio.valor > 100");
        Assert.That(ok, Is.True);
    }
}
