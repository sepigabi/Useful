public static bool IsIEnumerableOrArray(Type type)
{
    return type != typeof(string) && (type.IsArray || type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)));
}
