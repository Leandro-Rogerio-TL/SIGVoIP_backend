using AutoMapper;
using PainelIntegraTelefoniaIP.Entity;

namespace PainelIntegraTelefoniaIP.Models.Profiles
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<CreateClienteModel, Cliente>().ReverseMap();
            CreateMap<ReadClienteModel, Cliente>().ReverseMap();
        }
    }
}