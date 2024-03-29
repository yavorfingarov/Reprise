global using FluentValidation;
global using Microsoft.AspNetCore.Authorization;
global using Reprise.SampleApi.Data;
global using Reprise.SampleApi.ErrorHandling;
#if NET7_0
global using Reprise.SampleApi.Filters;
#endif

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "Logging performance is not a concern.")]
