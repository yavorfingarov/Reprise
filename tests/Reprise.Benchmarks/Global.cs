global using BenchmarkDotNet.Attributes;

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "BenchmarkDotNet doesn't support static methods.")]
