using System.Text.Json.Nodes;
using KUKULCAN.SharedKernel.JsonEngine.Graph;
using KUKULCAN.SharedKernel.JsonEngine.SQL;

namespace KUKULCAN.SharedKernel.JsonEngine.UnitTests.JsonAiEngine;

public class JsonAiQueryTests
{
    [Test]
    public void JsonAI_AIQueryParser_ShouldRemainStable()
    {
        JsonNode root = JsonNode.Parse("""
                                       {
                                         "items": [
                                           { "id": "A", "precio": { "valor": 120 } },
                                           { "id": "B", "precio": { "valor": 5   } },
                                           { "id": "C", "precio": { "valor": 40  } },
                                           { "id": "D", "precio": { "valor": 80  } }
                                         ]
                                       }
                                       """)!;

        var graph = new JsonGraph();
        graph.Build(root, "$.items", "id", "precio.valor");

        var sql = new JsonSqlEngine();

        // -----------------------------
        // 3 variantes de la misma AI QUERY
        // -----------------------------
        string[] queries =
        [
            """
            AI QUERY:
              FIND items WHERE precio.valor > 20
              THEN ORDER BY precio.valor DESC
              THEN LIMIT 2
              RETURN id
            """,

            """
            AI QUERY:
            FIND   items   WHERE   precio.valor   >   20
            THEN   ORDER BY   precio.valor   DESC
            THEN   LIMIT   2
            RETURN   id
            """,

            """
            AI QUERY:
            FIND items
            WHERE precio.valor > 20
            THEN ORDER BY precio.valor DESC
            THEN LIMIT 2
            RETURN id
            """
        ];

        List<List<string>> results = queries
            .Select(q => AI.JsonAiEngine.Execute(root, graph, q, sql))
            .Select(r => r.Select(x => x["id"]!.ToString().Trim('"')).ToList())
            .ToList();

        // -----------------------------
        // Todas las variantes deben producir el mismo resultado
        // -----------------------------
        Assert.That(results[1], Is.EqualTo(results[0]));
        Assert.That(results[2], Is.EqualTo(results[1]));

        // Validación del contenido
        Assert.That(results[0][0], Is.EqualTo("A")); // 120
        Assert.That(results[0][1], Is.EqualTo("D")); // 80
    }
}
