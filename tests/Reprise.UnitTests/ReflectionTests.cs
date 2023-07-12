using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public class ReflectionTests
    {
        [Fact]
        public Task PublicTypes()
        {
            return Verify(GetDescription(typeof(EndpointAttribute).Assembly, t => t.IsPublic))
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task NonPublicTypes()
        {
            var description = GetDescription(typeof(EndpointAttribute).Assembly,
                t => !t.IsPublic && !t.IsDefined(typeof(CompilerGeneratedAttribute)) && t.FullName!.StartsWith("Reprise", StringComparison.InvariantCulture));
            return Verify(description)
                .UniqueForRuntimeAndVersion();
        }

        private static object GetDescription(Assembly assembly, Func<Type, bool> typePredicate)
        {
            var types = assembly
                .GetTypes()
                .Where(typePredicate)
                .ToList();
            var description = new
            {
                Interface = types.Where(t => t.IsInterface).Select(t => t.FullName).ToList(),
                Abstract = types.Where(t => t.IsClass && t.IsAbstract && !t.IsSealed).Select(t => t.FullName).ToList(),
                Sealed = types.Where(t => t.IsClass && !t.IsAbstract && t.IsSealed).Select(t => t.FullName).ToList(),
                Open = types.Where(t => t.IsClass && !t.IsAbstract && !t.IsSealed).Select(t => t.FullName).ToList(),
                Static = types.Where(t => t.IsClass && t.IsAbstract && t.IsSealed).Select(t => t.FullName).ToList()
            };
            var reflectedCount = description.Interface.Count + description.Abstract.Count + description.Sealed.Count +
                description.Open.Count + description.Static.Count;

            Assert.Equal(types.Count, reflectedCount);

            return description;
        }
    }
}
