namespace AlgoApp
{
    static class ObjectMapper
    {
        public static T Map<T>(object originVariable) where T : new()
        {
            return Map(originVariable, new T());
        }

        public static T Map<T>(object originVariable, T newVariable)
        {
            var otype = originVariable.GetType();
            var ops = otype.GetProperties();
            var ntype = newVariable.GetType();
            foreach (var op in ops)
            {
                var info = ntype.GetProperty(op.Name);
                info?.SetValue(newVariable, op.GetValue(originVariable));
            }

            return newVariable;
        }
    }
}
