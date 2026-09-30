using System.Text.Json.Nodes;
using KUKULCAN.SharedKernel.JsonEngine.AI;
using KUKULCAN.SharedKernel.JsonEngine.Graph;
using KUKULCAN.SharedKernel.JsonEngine.SQL;

namespace KUKULCAN.SharedKernel.JsonEngine.Integration;

public class JsonAiEngineIntegrationTests
{
    private readonly JsonNode _root;
    private readonly JsonGraph _graph;
    private readonly JsonSqlEngine _sql;

    public JsonAiEngineIntegrationTests()
    {
        // JSON real
        _root = JsonCore.Parse("""
                               {
                                 "items": [
                                   { "id": "A", "nombre": "Motor",   "precio": { "valor": 120 }, "dependencias": ["B", "C"] },
                                   { "id": "B", "nombre": "Tornillo","precio": { "valor": 5   }, "dependencias": [] },
                                   { "id": "C", "nombre": "Eje",     "precio": { "valor": 40  }, "dependencias": ["B"] },
                                   { "id": "D", "nombre": "Filtro",  "precio": { "valor": 80  }, "dependencias": ["C"] }
                                 ]
                               }
                               """)!;

        // Grafo real
        _graph = new JsonGraph();
        _graph.Build(_root, "$.items", "id", "dependencias");

        // Motor SQL real
        _sql = new JsonSqlEngine();
    }

    [Test]
    public void AIQueryParser_ShouldRemainStable()
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

        List<List<string>> results = queries.Select(q => JsonAiEngine.Execute(root, graph, q, sql)).Select(r => r.Select(x => x["id"]!.ToString().Trim('"')).ToList()).ToList();

        Assert.That(results[1], Is.EqualTo(results[0]));
        Assert.That(results[2], Is.EqualTo(results[1]));

        Assert.That(results[0][0], Is.EqualTo("A"));
        Assert.That(results[0][1], Is.EqualTo("D"));
    }
    [Test]
    public void Conditions_ShouldFilterCorrectly()
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

        const string aiQuery = """
                               AI QUERY:
                                 FIND items WHERE precio.valor > 20 AND precio.valor <> 80
                                 RETURN id, precio.valor
                               """;

        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);

        List<string> ids = result.Select(r => r["id"]!.ToString().Trim('"')).ToList();

        Assert.That(ids, Does.Contain("A"));
        Assert.That(ids, Does.Contain("C"));
        Assert.That(ids, Does.Not.Contain("D"));
        Assert.That(ids, Does.Not.Contain("B"));
    }
    [Test]
    public void OrderByAndLimit_ShouldSortAndLimitCorrectly()
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

        const string aiQuery = """
                               AI QUERY:
                                 FIND items
                                 THEN ORDER BY precio.valor DESC
                                 THEN LIMIT 2
                                 RETURN id, precio.valor
                               """;

        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0]["id"]!.ToString().Trim('"'), Is.EqualTo("A"));
        Assert.That(result[1]["id"]!.ToString().Trim('"'), Is.EqualTo("D"));

    }
    [Test]
    public void ReturnAll_ShouldReturnRawJsonNodes()
    {
        JsonNode root = JsonNode.Parse("""
                                       {
                                         "items": [
                                           { "id": "A", "nombre": "Motor", "precio": { "valor": 120 } },
                                           { "id": "B", "nombre": "Tornillo", "precio": { "valor": 5 } }
                                         ]
                                       }
                                       """)!;

        var graph = new JsonGraph();
        graph.Build(root, "$.items", "id", "precio.valor");

        var sql = new JsonSqlEngine();

        const string aiQuery = """
                               AI QUERY:
                                 FIND items
                                 RETURN *
                               """;

        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0]["*"]!.ToString(), Is.EqualTo(root["items"]![0]!.ToString()));
        Assert.That(result[1]["*"]!.ToString(), Is.EqualTo(root["items"]![1]!.ToString()));
    }
    [Test]
    public void StressTest_10000Nodes_50000Edges()
    {
        const int nodeCount = 10000;
        const int edgeCount = 50000;

        var items = new JsonArray();
        for (int i = 0; i < nodeCount; i++)
        {
            items.Add(new JsonObject
            {
                ["id"] = $"N{i}",
                ["valor"] = i,
                ["dependencias"] = new JsonArray()
            });
        }

        var rnd = new Random(42);
        for (int i = 0; i < edgeCount; i++)
        {
            int from = rnd.Next(nodeCount);
            int to = rnd.Next(nodeCount);
            if (from != to)
                ((JsonArray)items[from]!["dependencias"]!).Add($"N{to}");
        }

        var root = new JsonObject { ["items"] = items };

        var graph = new JsonGraph();
        graph.Build(root, "$.items", "id", "dependencias");

        var sql = new JsonSqlEngine();

        const string aiQuery = """
                               AI QUERY:
                                 FIND items WHERE valor > 5000
                                 THEN GRAPH EXPAND dependencias[*] UP TO 2 LEVELS
                                 THEN RANK BY PAGERANK
                                 RETURN id, valor
                               """;

        var sw = System.Diagnostics.Stopwatch.StartNew();
        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);
        sw.Stop();

        Assert.That(result, Is.Not.Empty);
        Assert.That(sw.ElapsedMilliseconds < 2000, Is.True, $"Tardó {sw.ElapsedMilliseconds} ms");
    }
    [Test]
    public void JsonAI_EndToEnd_ShouldReturnRankedExpensiveItems()
    {
        // Lenguaje natural → AI QUERY
        const string nl = "Muéstrame los items caros y sus dependencias ordenadas por importancia";
        string aiQuery = JsonAiNl.ToAiQuery(nl);

        // Ejecutar pipeline completo
        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(_root, _graph, aiQuery, _sql);

        // Validaciones
        Assert.That(result, Is.Not.Empty);

        // Extraer IDs
        List<string> ids = result.Select(r => r["id"]!.ToString().Trim('"')).ToList();

        // Items caros: A (120), D (80), C (40)
        Assert.That(ids, Does.Contain("A"));
        Assert.That(ids, Does.Contain("D"));
        Assert.That(ids, Does.Contain("C"));

        // PageRank real: A > C > D > B
        int indexA = ids.IndexOf("A");
        int indexC = ids.IndexOf("C");
        int indexD = ids.IndexOf("D");

        Assert.That(indexA < indexC, Is.True);
        Assert.That(indexD < indexC, Is.True);
    }
    [Test]
    public void JsonAI_Conditions_ShouldFilterCorrectly()
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

        const string aiQuery = """
                               AI QUERY:
                                 FIND items WHERE precio.valor > 20 AND precio.valor <> 80
                                 RETURN id, precio.valor
                               """;

        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);

        List<string> ids = result.Select(r => r["id"]!.ToString().Trim('"')).ToList();

        Assert.That(ids, Does.Contain("A")); // 120
        Assert.That(ids, Does.Contain("C")); // 40
        Assert.That(ids, Does.Not.Contain("D")); // 80 excluido
        Assert.That(ids, Does.Not.Contain("B")); // 5 excluido
    }
    [Test]
    public void JsonAI_OrderByAndLimit_ShouldSortAndLimitCorrectly()
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

        const string aiQuery = """
                               AI QUERY:
                                 FIND items
                                 THEN ORDER BY precio.valor DESC
                                 THEN LIMIT 2
                                 RETURN id, precio.valor
                               """;

        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0]["id"]!.ToString().Trim('"'), Is.EqualTo("A")); // 120
        Assert.That(result[1]["id"]!.ToString().Trim('"'), Is.EqualTo("D")); // 80
    }
    [Test]
    public void JsonAI_ReturnAll_ShouldReturnRawJsonNodes()
    {
        JsonNode root = JsonNode.Parse("""
                                       {
                                         "items": [
                                           { "id": "A", "nombre": "Motor", "precio": { "valor": 120 } },
                                           { "id": "B", "nombre": "Tornillo", "precio": { "valor": 5 } }
                                         ]
                                       }
                                       """)!;

        var graph = new JsonGraph();
        graph.Build(root, "$.items", "id", "precio.valor");

        var sql = new JsonSqlEngine();

        const string aiQuery = """
                               AI QUERY:
                                 FIND items
                                 RETURN *
                               """;

        List<Dictionary<string, JsonNode?>> result = JsonAiEngine.Execute(root, graph, aiQuery, sql);

        Assert.That(result.Count, Is.EqualTo(2));

        // Debe devolver el nodo completo
        Assert.That(result[0]["*"]!.ToString(), Is.EqualTo(root["items"]![0]!.ToString()));
        Assert.That(result[1]["*"]!.ToString(), Is.EqualTo(root["items"]![1]!.ToString()));
    }

}
