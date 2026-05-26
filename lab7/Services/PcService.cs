using lab7.Data;
using lab7.DTOs;
using lab7.Models;
using Microsoft.EntityFrameworkCore;

namespace lab7.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _context;

    public PcService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PcGetDto>> GetAllAsync()
    {
        return await _context.PCs
            .Select(pc => MapToGetDto(pc))
            .ToListAsync();
    }

    public async Task<PcWithComponentsDto?> GetWithComponentsAsync(int id)
    {
        if (!await _context.PCs.AnyAsync(pc => pc.Id == id))
        {
            return null;
        }

        return await _context.PCs
            .Where(pc => pc.Id == id)
            .Select(pc => new PcWithComponentsDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock,
                Components = pc.PCComponents.Select(pcComponent => new ComponentInPcDto
                {
                    Code = pcComponent.Component.Code,
                    Name = pcComponent.Component.Name,
                    Description = pcComponent.Component.Description,
                    Amount = pcComponent.Amount,
                    ComponentType = new ComponentTypeDto
                    {
                        Id = pcComponent.Component.ComponentType.Id,
                        Abbreviation = pcComponent.Component.ComponentType.Abbreviation,
                        Name = pcComponent.Component.ComponentType.Name
                    },
                    ComponentManufacturer = new ComponentManufacturerDto
                    {
                        Id = pcComponent.Component.ComponentManufacturer.Id,
                        Abbreviation = pcComponent.Component.ComponentManufacturer.Abbreviation,
                        FullName = pcComponent.Component.ComponentManufacturer.FullName,
                        FoundationDate = pcComponent.Component.ComponentManufacturer.FoundationDate
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PcGetDto> CreateAsync(PcCreateDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt!.Value,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return MapToGetDto(pc);
    }

    public async Task<PcGetDto?> UpdateAsync(int id, PcUpdateDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc is null)
        {
            return null;
        }

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt!.Value;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();

        return MapToGetDto(pc);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc is null)
        {
            return false;
        }

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();

        return true;
    }

    private static PcGetDto MapToGetDto(PC pc)
    {
        return new PcGetDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }
}
