using System;
using Marten;

namespace UsersService.DataAccess
{
    public interface IDocumentStoreFactory
    {
        IDocumentStore Store { get; }
    }

    class DocumentStoreFactory : IDocumentStoreFactory
    {
        private string _connectionString;

        public DocumentStoreFactory()
        {
            _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        }

        public IDocumentStore Store
        {
            get
            {
                return DocumentStore.For(_ => { _.Connection(_connectionString); });
            }
        }
    }
}