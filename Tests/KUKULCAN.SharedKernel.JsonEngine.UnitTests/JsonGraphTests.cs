using KUKULCAN.SharedKernel.JsonEngine.Graph;

namespace KUKULCAN.SharedKernel.JsonEngine.UnitTests;

public class JsonGraphTests
{
    private readonly JsonGraph _graph;
    private static readonly string[] sourceArray = new[] { "A", "B", "C" };
    private static readonly string[] expected = new[] { "A", "B" };

    public JsonGraphTests()
    {
        const string json = """
                            {
                              "items": [
                                { "id": "A", "dependencias": ["B", "C"] },
                                { "id": "B", "dependencias": [] },
                                { "id": "C", "dependencias": ["B"] }
                              ]
                            }
                            """;

        var root = JsonCore.Parse(json)!;

        _graph = new JsonGraph();
        _graph.Build(root, "$.items", "id", "dependencias");
    }

    [Test]
    public void Graph_ShouldBuildNodesAndEdges()
    {
        Assert.That(_graph.Nodes.Count, Is.EqualTo(3));
        Assert.That(_graph.Edges.Count, Is.EqualTo(3));
    }

    [Test]
    public void BFS_ShouldReturnTraversal()
    {
        var bfs = _graph.Bfs("A");
        Assert.That(bfs.OrderBy(x => x), Is.EqualTo(sourceArray.OrderBy(x => x)));
    }

    [Test]
    public void FindPath_ShouldReturnCorrectPath()
    {
        var path = _graph.FindPath("A", "B");
        Assert.That(path, Is.EqualTo(expected));
    }

    [Test]
    public void PageRank_ShouldReturnScores()
    {
        var pr = _graph.PageRank();
        Assert.That(pr.Count, Is.EqualTo(3));
    }

    [Test]
    public void Build_ShouldStoreScalarIdentifiersWithoutJsonQuotes()
    {
        Assert.That(_graph.Nodes.Keys, Does.Contain("A"));
        Assert.That(_graph.Bfs("A"), Does.Contain("B"));
    }

    [Test]
    public void Build_ShouldReplacePreviousGraphState()
    {
        var root = JsonCore.Parse("""{ "items": [{ "id": "X", "dependencias": [] }] }""")!;
        var graph = new JsonGraph();
        graph.Build(root, "$.items", "id", "dependencias");
        Assert.That(graph.Nodes.Keys, Is.EquivalentTo(new[] { "X" }));
    }

    [Test]
    public void Dfs_ShouldReturnEmptyForUnknownNode() => Assert.That(_graph.Dfs("UNKNOWN"), Is.Empty);

    [Test]
    public void PageRank_ShouldHandleEmptyGraph() => Assert.That(new JsonGraph().PageRank(), Is.Empty);
}
