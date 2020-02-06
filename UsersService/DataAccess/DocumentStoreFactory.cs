using System;
using Marten;

namespace UsersService.DataAccess
{
    class DocumentStoreFactory : IDocumentStoreFactory
    {
        private readonly string _connectionString;

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