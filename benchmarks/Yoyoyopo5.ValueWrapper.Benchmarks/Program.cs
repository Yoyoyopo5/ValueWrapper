using BenchmarkDotNet.Running;
using System.ComponentModel;
using Yoyoyopo5.ValueWrapper.Benchmarks;
using Yoyoyopo5.ValueWrapper.Benchmarks.Create;
using Yoyoyopo5.ValueWrapper.Benchmarks.Json;
using Yoyoyopo5.ValueWrapper.Benchmarks.ToString;
using Yoyoyopo5.ValueWrapper.Benchmarks.TypeConv;

//JsonBenchmarks<ColorName, string>.Run();
//CreateBenchmarks<ColorName, string>.Run();
//ToStringBenchmarks<ColorName, string>.Run();
//TypeConverterBenchmarks<ColorName, string>.Run();
BenchmarkRunner.Run<DictionaryKeyJsonBenchmarks>();