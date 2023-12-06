using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories
{
    public class RaceDbRepository(ApplicationDbContext context) : IRaceRepository
    {
        public IQueryable<Race> Entities => context.Races;

        public async Task<Race?> FindByIdAsync(int id) => await Entities.SingleOrDefaultAsync(r => r.Id == id);

        public async Task<Race> AddAsync(Race entity)
        {
            entity.Id = 0;
            Validate(entity);
            var entry = await context.AddAsync(entity);

            await context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task UpdateAsync(int id, Race entity)
        {
            var raceToEdit = await Entities.SingleAsync(r => r.Id == id);
            entity.CopyTo(raceToEdit);

            Validate(raceToEdit);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var raceToDelete = await Entities.SingleAsync(r => r.Id == id);
            context.Races.Remove(raceToDelete);
            await context.SaveChangesAsync();
        }

        private static void Validate(Race race)
        {
            var validationContext = new ValidationContext(race);
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(race, validationContext, errors, true);

            if (errors.Count != 0)
                throw new InvalidModelException($"Given model of type {nameof(Race)} is invalid.", errors);
        }
    }
}
