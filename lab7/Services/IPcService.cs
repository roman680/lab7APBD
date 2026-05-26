using lab7.DTOs;

namespace lab7.Services;

public interface IPcService
{
    Task<List<PcGetDto>> GetAllAsync();
    Task<PcWithComponentsDto?> GetWithComponentsAsync(int id);
    Task<PcGetDto> CreateAsync(PcCreateDto dto);
    Task<PcGetDto?> UpdateAsync(int id, PcUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
