using BenchmarkDotNet.Running;

using ValidationBenchmark;

// See https://aka.ms/new-console-template for more information

Console.WriteLine("Iniciando Benchmark...");
var summary = BenchmarkRunner.Run<ClienteValidationBenchmark>();
Console.WriteLine("Benchmark concluído.");