using System.Text.Json.Nodes;
using KUKULCAN.SharedKernel.JsonEngine.AI;
using KUKULCAN.SharedKernel.JsonEngine.Graph;
using KUKULCAN.SharedKernel.JsonEngine.SQL;

namespace KUKULCAN.SharedKernel.JsonEngine.Integration;

public class JsonGraphIntegrationTests
{
    [Test]
    public void JsonAI_GraphExpand_WithLevels_ShouldExpandCorrectly()
    {
        JsonNode root = JsonNode.Parse("""
                                       {
                                         "items": [
                                           { "id": "A", "dependencias": ["B", "C"] },
                                           { "id": "B", "dependencias": ["D"] },
                                           { "id": "C", "dependencias": [] },
                                           { "id": "D", "dependencias": [] }
                                         ]
                                       }
                                       """)!;

        var graph = new JsonGraph();
        graph.Build(root, "$.items", "id", "dependencias");

        var sql = new JsonSqlEngine();

        const string aiQuery = """
                               AI QUERY:
                                 FIND items WHERE id = "A"
                                 THEN GRAPH EXPAND dependencias[*] UP TO 2 LEVELS
                                 RETURN id
                               """;

        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);

        List<string> ids = result.Select(r => r["id"]!.ToString().Trim('\"')).ToList();

        Assert.Contains("A", ids);
        Assert.Contains("B", ids);
        Assert.Contains("C", ids);
        Assert.Contains("D", ids); // B → D (2 niveles)
    }
    [Test]
    public void GraphExpand_WithLevels_ShouldExpandCorrectly()
    {
        JsonNode root = JsonNode.Parse("""
                                       {
                                         "items": [
                                           { "id": "A", "dependencias": ["B", "C"] },
                                           { "id": "B", "dependencias": ["D"] },
                                           { "id": "C", "dependencias": [] },
                                           { "id": "D", "dependencias": [] }
                                         ]
                                       }
                                       """)!;

        var graph = new JsonGraph();
        graph.Build(root, "$.items", "id", "dependencias");

        var sql = new JsonSqlEngine();

        const string aiQuery = """
                               AI QUERY:
                                 FIND items WHERE id = "A"
                                 THEN GRAPH EXPAND dependencias[*] UP TO 2 LEVELS
                                 RETURN id
                               """;

        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);

        List<string> ids = result.Select(r => r["id"]!.ToString().Trim('\"')).ToList();

        Assert.Contains("A", ids);
        Assert.Contains("B", ids);
        Assert.Contains("C", ids);
        Assert.Contains("D", ids);
    }
}
