using System.Text.Json.Nodes;
using KUKULCAN.SharedKernel.JsonEngine.Graph;
using KUKULCAN.SharedKernel.JsonEngine.SQL;
using Moq;

namespace KUKULCAN.SharedKernel.JsonEngine.UnitTests.JsonAiEngine;

public class JsonAiEngineMoqTests
{
    [Test]
    public void Execute_ShouldUseMocksCorrectly()
    {
        // JSON raíz simulado
        var root = JsonNode.Parse("""
        {
          "items": [
            { "id": "A", "nombre": "Motor" },
            { "id": "B", "nombre": "Tornillo" }
          ]
        }
        """);

        // -----------------------------
        // MOCK DEL GRAFO (IJsonGraph)
        // -----------------------------
        var mockGraph = new Mock<IJsonGraph>();

        mockGraph.SetupGet(g => g.Nodes).Returns(new Dictionary<string, JsonNode>
        {
            ["A"] = root!["items"]![0]!,
            ["B"] = root["items"]![1]!
        });

        mockGraph.SetupGet(g => g.Edges).Returns(new Dictionary<string, List<string>>
        {
            ["A"] = [],
            ["B"] = []
        });

        mockGraph.Setup(g => g.PageRank(It.IsAny<int>(), It.IsAny<double>()))
            .Returns(new Dictionary<string, double>
            {
                ["A"] = 0.9,
                ["B"] = 0.1
            });

        // -----------------------------
        // MOCK DEL MOTOR SQL
        // -----------------------------
        var mockSql = new Mock<IJsonSqlEngine>();

        mockSql.Setup(s => s.Execute(It.IsAny<JsonNode>(), It.IsAny<string>()))
            .Returns([
                new JsonSqlRow { ["id"] = root["items"]![0]!["id"], ["nombre"] = root["items"]![0]!["nombre"] },
                new JsonSqlRow { ["id"] = root["items"]![1]!["id"], ["nombre"] = root["items"]![1]!["nombre"] }
            ]);

        // -----------------------------
        // AI QUERY
        // -----------------------------
        const string aiQuery = """
        AI QUERY:
          FIND items[*]
          THEN RANK BY PAGERANK
          RETURN id, nombre
        """;

        // -----------------------------
        // EJECUCIÓN DEL PIPELINE
        // -----------------------------
        List<Dictionary<string, JsonNode?>> result = AI.JsonAiEngine.Execute(
            root,
            mockGraph.Object,
            aiQuery,
            mockSql.Object
        );

        // -----------------------------
        // ASSERTS
        // -----------------------------
        Assert.That(result.Count, Is.EqualTo(2));          // Debe devolver 2 filas
        Assert.That(result[0]["id"]!.ToString().Trim('"'), Is.EqualTo("A")); // A debe ir primero por PageRank
    }
}
