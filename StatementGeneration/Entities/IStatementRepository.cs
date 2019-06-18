using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatementGeneration.Entities
{
    public interface IStatementRepository
    {
        Task<int> GenerateStatement(StatementRequest request);
        Task<List<StatementResponse>> FilterStatement(StatementRequest request);
        string EncData(string value);
    }
}
