using System.Globalization;

namespace Reprise
{
    internal sealed class TagsProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            var tagsAttribute = handlerInfo.GetCustomAttribute<TagsAttribute>();
            if (tagsAttribute != null)
            {
                if (tagsAttribute.Tags.Any(t => t.IsEmpty()))
                {
                    throw new InvalidOperationException($"{handlerInfo.GetFullName()} has an empty tag.");
                }
                builder.WithTags(tagsAttribute.Tags);
            }
            else
            {
                builder.WithTags(CreateTag(route));
            }
        }

        private static string CreateTag(string route)
        {
            return route.Split('/', StringSplitOptions.RemoveEmptyEntries)
                .SkipWhile(IsNotMeaningful)
                .Select(Capitalize)
                .FirstOrDefault("/");
        }

        private static bool IsNotMeaningful(string token)
        {
            return token.Equals("api", StringComparison.OrdinalIgnoreCase) || token.StartsWith('{') || token.IsEmpty();
        }

        private static string Capitalize(string input)
        {
            var characters = input.Select((c, i) => (i == 0) ? char.ToUpper(c, CultureInfo.InvariantCulture) : c);

            return new string(characters.ToArray());
        }
    }
}
