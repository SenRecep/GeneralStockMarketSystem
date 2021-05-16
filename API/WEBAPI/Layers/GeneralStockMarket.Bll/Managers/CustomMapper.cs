using System.Linq;
using System.Reflection;

using GeneralStockMarket.Bll.Interfaces;

namespace GeneralStockMarket.Bll.Managers
{
    public class CustomMapper : ICustomMapper
    {
        public T Map<T, D>(D dto, T entity)
        {
            PropertyInfo[] entityProperties = entity.GetType().GetProperties();
            PropertyInfo[] dtoTypeProperties = dto.GetType().GetProperties();

            foreach (var dtoProperty in dtoTypeProperties)
            {
                PropertyInfo entityProperty = entityProperties
                    .FirstOrDefault(x =>
                    x.Name.Equals(dtoProperty.Name) &&
                    x.GetType().Equals(dtoProperty.GetType()));

                entityProperty.SetValue(entity, dtoProperty.GetValue(dto));
            }
            return entity;

        }
    }
}
