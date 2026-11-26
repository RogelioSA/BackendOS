using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace nest.core.security.Extensions
{
    public sealed class CleanODataContentTypesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var response in operation.Responses.Values)
                CleanContentTypes(response.Content);

            if (operation.RequestBody is not null)
                CleanContentTypes(operation.RequestBody.Content);
        }

        private static void CleanContentTypes(
            IDictionary<string, OpenApiMediaType> content)
        {
            if (content is null || content.Count == 0)
                return;

            const string target = "application/json";

            // Conservar el JSON simple si existe.
            // De lo contrario, tomar una variante OData.
            var jsonMediaType = content.TryGetValue(target, out var plain)
                ? plain
                : content.FirstOrDefault(x =>
                    x.Key.StartsWith(
                        target + ";",
                        StringComparison.OrdinalIgnoreCase)).Value;

            if (jsonMediaType is null)
                return;

            // Eliminar únicamente las variantes JSON con parámetros OData.
            var odataKeys = content.Keys
                .Where(key => !key.Equals(target, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var key in odataKeys)
                content.Remove(key);

            // Registrar JSON simple conservando el objeto original.
            if (!content.ContainsKey(target))
                content[target] = jsonMediaType;
        }
    }
}
