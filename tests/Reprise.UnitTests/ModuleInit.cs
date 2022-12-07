using System.Runtime.CompilerServices;

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
