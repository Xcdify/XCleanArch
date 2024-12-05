using AutoMapper;
using XMP.Application.DTOs;
using XMP.Domain.Entities;


namespace XMP.Application.Mappers
{
    public class AxisBankTransactionProfile : Profile
    {
        public AxisBankTransactionProfile()
        {
            CreateMap<AxisBankTransaction, AxisBankTransactionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.ValueDate, opt => opt.MapFrom(src => src.ValueDate))
                .ForMember(dest => dest.ChequeNumber, opt => opt.MapFrom(src => src.ChequeNumber))
                .ForMember(dest => dest.TransactionDetails, opt => opt.MapFrom(src => src.TransactionDetails))
                .ForMember(dest => dest.AmountTransferred, opt => opt.MapFrom(src => src.AmountTransferred))
                .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.TypeOfTransaction))
                .ForMember(dest => dest.BalanceAmount, opt => opt.MapFrom(src => src.BalanceAmount))
                .ForMember(dest => dest.BankBranchName, opt => opt.MapFrom(src => src.BankBranchName))
                .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.ReceiverName))
                .ForMember(dest => dest.TransactionCategory, opt => opt.MapFrom(src => src.TransactionCategory))
                .ForMember(dest => dest.TypeOfTax, opt => opt.MapFrom(src => src.TypeOfTax))
                .ForMember(dest => dest.TaxPercentage, opt => opt.MapFrom(src => src.TaxPercentage))
                .ReverseMap(); // For reverse mapping if needed
        }
    }
}
