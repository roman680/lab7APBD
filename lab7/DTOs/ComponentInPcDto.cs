namespace lab7.DTOs;

public class ComponentInPcDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Amount { get; set; }
    public ComponentTypeDto ComponentType { get; set; } = null!;
    public ComponentManufacturerDto ComponentManufacturer { get; set; } = null!;
}
