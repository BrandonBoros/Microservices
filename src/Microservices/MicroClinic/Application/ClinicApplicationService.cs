using Microsoft.Extensions.Caching.Memory;

namespace MicroClinic.Application;
using MicroClinic.Controllers;
using MicroClinic.DataAccess;
using MicroClinic.ExternalServices;

public class ClinicApplicationService(ClinicDbContext dbContext,
    ManagementService managementService, IMemoryCache memoryCache)
{
    public async Task<Consultation> Handle(StartConsultationCommand command)
    {
        PetInfo? petInfo = await memoryCache.GetOrCreateAsync(command.PatientId,
            async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await managementService.GetPetInfo(command.PatientId);
            });

        var newConsultation = new Consultation(Guid.NewGuid(),
            command.PatientId,
            petInfo.Name,
            petInfo.Age,
            DateTime.UtcNow);
        await dbContext.Consultations.AddAsync(newConsultation);
        await dbContext.SaveChangesAsync();

        return newConsultation;
    }
}