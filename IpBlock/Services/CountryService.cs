using IpBlock.Exceptions;
using IpBlock.Models;
using IpBlock.Models.DTOs.Request;
using IpBlock.Models.DTOs.Response;
using IpBlock.Repositories;

namespace IpBlock.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repo;

        public CountryService(ICountryRepository repo)
        {
            _repo = repo;
        }

        public BlockCountryEntity AddBlocked(BlockCountryRequest request)
        {
            if (_repo.IsBlocked(request.CountryCode))
                throw new ConflictException("Country already blocked!");

            BlockCountryEntity entity = new BlockCountryEntity(request.CountryCode, request.Name);

            return entity;
        }

        public BlockCountryEntity AddBlocked(TemporalBlockRequest request)
        {
            if (_repo.IsBlocked(request.CountryCode))
                throw new ConflictException("Country already blocked!");

            BlockCountryEntity entity = new BlockCountryEntity(request.CountryCode, request.Name);
            entity.IsTemporal = true;
            entity.ExpiresAt = DateTime.Now.AddMinutes(request.DurationMinutes);

            if (!_repo.AddBlocked(entity))
                if (_repo.RemoveBlocked(request.CountryCode))
                    if (!_repo.AddBlocked(entity))
                        throw new InternalServerErrorException("Data not saved!");

            return entity;
        }

        public PagedResponse<object> GetAll(int page, int pageSize, string? q) => _repo.GetAll(page, pageSize, q);

        public bool RemoveBlocked(string countryCode)
        {
            if (!_repo.RemoveBlocked(countryCode))
                throw new NotFoundException("Country Not found!");
            
            return true;
        }
    }
}
