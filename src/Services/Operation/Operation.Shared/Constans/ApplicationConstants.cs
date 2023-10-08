namespace Operation.Shared.Constans;

public static class ApplicationConstants
{
    public static class CosmosDB
    {
        public const string CONTAINER_OPERATION = "Operation";
    }

    public static class RabbitMq
    {
        public const string EXCHANGE_OPERATION = "EXCHANGE_OPERATION";
    }

    public static class Cache
    {
        public const string OPERATION_KEY = "operation";
        public const string OPERATIONATTRIBUTE_KEY = "operationAttribute";
        public const string OPERATIONWITHATTRIBUTE_KEY = "operationWithAttribute";
    }
}