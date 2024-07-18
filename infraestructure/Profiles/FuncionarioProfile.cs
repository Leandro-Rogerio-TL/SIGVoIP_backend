using AutoMapper;
using PainelIntegraTelefoniaIP.Entity;

namespace PainelIntegraTelefoniaIP.Models.Profiles
{
    public class FuncionarioProfile : Profile
    {
        public FuncionarioProfile()
        {
            CreateMap<CreateFuncionarioModel, Funcionario>().ReverseMap();
            CreateMap<ReadFuncionarioModel, Funcionario>().ReverseMap();
        }
    }
}