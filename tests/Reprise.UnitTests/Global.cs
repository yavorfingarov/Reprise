global using FluentValidation;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Moq;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test names can contain underscores.")]
[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Plain exception is used in tests.")]
[assembly: SuppressMessage("Build", "CA1852", Justification = "Using unsealed internal types in tests is not problematic.")]

namespace Reprise.UnitTests
{
    public class ModuleInit
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifyMoq.Enable();
            VerifyHttp.Enable();
            VerifyMicrosoftLogging.Enable();
        }
    }
}
