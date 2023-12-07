using Microsoft.EntityFrameworkCore;

namespace PIMTool.Core.Interfaces;

public interface IModelMapper
{
    void Mapping(ModelBuilder modelBuilder);
}