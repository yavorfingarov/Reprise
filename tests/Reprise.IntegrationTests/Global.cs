using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test names can contain underscores.")]

namespace Reprise.IntegrationTests
{
    public class ModuleInit
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifyHttp.Initialize();
        }
    }
}
