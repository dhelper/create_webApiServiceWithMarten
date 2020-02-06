using Marten;

namespace UsersService.DataAccess
{
    public interface IDocumentStoreFactory
    {
        IDocumentStore Store { get; }
    }
}