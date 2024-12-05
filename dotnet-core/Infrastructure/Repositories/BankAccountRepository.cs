using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using XMP.Domain.Entities;
using XMP.Domain.Repositories;
using XMP.Infrastructure.DbContext;

namespace XMP.Infrastructure.Repositories
{
    public class AxisBankTransactionRepository : IAxisBankTransactionRepository
    {
        private readonly DapperDbContext _dbContext;

        public AxisBankTransactionRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<AxisBankTransaction>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("PageNumber and PageSize must be positive integers.");
            }

            using var connection = _dbContext.GetConnection();
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;
            var query = $"SELECT * FROM axisbank_transactions ORDER BY id OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return await connection.QueryAsync<AxisBankTransaction>(query);
        }

        public async Task<AxisBankTransaction> GetByIdAsync(int id)
        {
            using var connection = _dbContext.GetConnection();
            var query = "SELECT * FROM axisbank_transactions WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<AxisBankTransaction>(query, new { Id = id });
        }

        public async Task AddAsync(AxisBankTransaction transaction)
        {
            using var connection = _dbContext.GetConnection();
            var query = @"INSERT INTO axisbank_transactions 
                        (transactiondate, valuedate, chequenumber, transactiondetails, amounttransferred, 
                         typeoftransaction, balanceamount, bankbranchname, bankname, companyname, 
                         receivername, transactioncategory, typeoftax, taxpercentage) 
                         VALUES 
                        (@TransactionDate, @ValueDate, @ChequeNumber, @TransactionDetails, @AmountTransferred, 
                         @TypeOfTransaction, @BalanceAmount, @BankBranchName, @BankName, @CompanyName, 
                         @ReceiverName, @TransactionCategory, @TypeOfTax, @TaxPercentage)";
            await connection.ExecuteAsync(query, transaction);
        }

        public async Task UpdateAsync(AxisBankTransaction transaction)
        {
            using var connection = _dbContext.GetConnection();
            var query = @"UPDATE axisbank_transactions SET 
                        transactiondate = @TransactionDate, valuedate = @ValueDate, chequenumber = @ChequeNumber, 
                        transactiondetails = @TransactionDetails, amounttransferred = @AmountTransferred, 
                        typeoftransaction = @TypeOfTransaction, balanceamount = @BalanceAmount, 
                        bankbranchname = @BankBranchName, bankname = @BankName, companyname = @CompanyName, receivername = @ReceiverName,
                        transactioncategory = @TransactionCategory,
                        typeoftax = @TypeOfTax,
                        taxpercentage = @TaxPercentage 
                        WHERE id = @Id";
            await connection.ExecuteAsync(query, transaction);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _dbContext.GetConnection();
            var query = "DELETE FROM axisbank_transactions WHERE id = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
