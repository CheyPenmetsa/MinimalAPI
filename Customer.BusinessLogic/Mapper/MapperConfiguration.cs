using Customer.BusinessLogic.DTOs;
using Mapster;

namespace Customer.BusinessLogic.Mapper
{
    public class MapperConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CustomerEntity, CustomerDto>();

            config.NewConfig<UpsertCustomerDto, CustomerEntity>();
        }
    }
}
