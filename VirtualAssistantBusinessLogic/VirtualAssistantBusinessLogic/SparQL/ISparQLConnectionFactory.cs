namespace VirtualAssistantBusinessLogic.SparQL
{
    public interface ISparQLConnectionFactory
    {
        public ISparQLConnection GetConnection();
    }
}
