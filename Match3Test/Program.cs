using BenchmarkDotNet.Running;
using Match3Test;

var summary = BenchmarkRunner.Run<Benchmark>();
