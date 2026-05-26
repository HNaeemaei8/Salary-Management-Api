using Application.Dto;


namespace Application.Interfaces
{
    public interface ISalaryCalculationService
    {
        Task<decimal> CalculateAsync(SalaryCalculationRequest request);

    }
}
