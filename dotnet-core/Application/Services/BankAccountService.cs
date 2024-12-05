using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using XMP.Application.DTOs;
using XMP.Domain.Entities;
using XMP.Domain.Repositories;
using XMP.Application.Interfaces;


namespace XMP.Application.Services
{
    public class AxisBankTransactionService : IAxisBankTransactionService
    {
        private readonly IAxisBankTransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public AxisBankTransactionService(IAxisBankTransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AxisBankTransactionDto>> GetAllTransactionsAsync(int pageNumber, int pageSize)
        {
            var transactions = await _transactionRepository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<AxisBankTransactionDto>>(transactions);
        }


        public async Task<AxisBankTransactionDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            return _mapper.Map<AxisBankTransactionDto>(transaction);
        }

        public async Task AddTransactionAsync(AxisBankTransactionDto transactionDto)
        {
            var transaction = _mapper.Map<AxisBankTransaction>(transactionDto);
            await _transactionRepository.AddAsync(transaction);
        }

        public async Task UpdateTransactionAsync(AxisBankTransactionDto transactionDto)
        {
            var transaction = _mapper.Map<AxisBankTransaction>(transactionDto);
            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteAsync(id);
        }
    }
}
