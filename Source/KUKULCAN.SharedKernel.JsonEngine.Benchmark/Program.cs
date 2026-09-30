using BenchmarkDotNet.Running;

namespace KUKULCAN.SharedKernel.JsonEngine.Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<JsonEngineBenchmarks>();
    }
}
