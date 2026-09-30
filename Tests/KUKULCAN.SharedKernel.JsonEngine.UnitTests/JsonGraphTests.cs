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
        Assert.Equal(3, _graph.Nodes.Count);
        Assert.Equal(3, _graph.Edges.Count);
    }

    [Test]
    public void BFS_ShouldReturnTraversal()
    {
        var bfs = _graph.Bfs("A");
        Assert.Equal(sourceArray.OrderBy(x => x), bfs.OrderBy(x => x));
    }

    [Test]
    public void FindPath_ShouldReturnCorrectPath()
    {
        var path = _graph.FindPath("A", "B");
        Assert.Equal(expected, path);
    }

    [Test]
    public void PageRank_ShouldReturnScores()
    {
        var pr = _graph.PageRank();
        Assert.Equal(3, pr.Count);
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
        _graph.Build(root, "$.items", "id", "dependencias");
        Assert.That(_graph.Nodes.Keys, Is.EquivalentTo(new[] { "X" }));
    }

    [Test]
    public void Dfs_ShouldReturnEmptyForUnknownNode() => Assert.That(_graph.Dfs("UNKNOWN"), Is.Empty);

    [Test]
    public void PageRank_ShouldHandleEmptyGraph() => Assert.That(new JsonGraph().PageRank(), Is.Empty);
}
