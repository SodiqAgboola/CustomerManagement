using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.HelperMethods
{
    public static class QueryMethods
    {
        public static IQueryable<TEntity> GetData<TEntity, TViewModel>(this IQueryable<TEntity> entities, TViewModel vm)
        {

            var query = entities;
            var properties = vm.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propValue = property.GetValue(vm, null);


                if (propValue == null) continue;

                if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    query = query.Where(e => EF.Property<int>(e, property.Name) == (int)propValue);
                }
                else if (property.PropertyType == typeof(string))
                {
                    if ((string)propValue == string.Empty) continue;

                    string queryvalue = (string)propValue;
                    query = query.Where(e => EF.Functions.Like(EF.Property<string>(e, property.Name).ToLower(), "%" + queryvalue.ToLower() + "%"));
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    query = query.Where(e => EF.Property<bool>(e, property.Name) == (bool)propValue);
                }
            }

            //dynamic date query
            var fromDateProperty = (DateTime?)properties.FirstOrDefault(x => x.Name == "FromDate")?.GetValue(vm, null);
            var toDateProperty = (DateTime?)properties.FirstOrDefault(x => x.Name == "ToDate")?.GetValue(vm, null);


            if (fromDateProperty != null && toDateProperty != null)
            {
                query = query.Where(e =>
                    EF.Property<DateTime>(e, "DateCreated") >= fromDateProperty.Value &&
                    EF.Property<DateTime>(e, "DateCreated") <= toDateProperty.Value);
            }

            //datetime orderby desc
            //datetime is not an indexed field so this was switched to use Id ordering
            query = query.OrderByDescending(e => EF.Property<int>(e, "Id"));

            return query;
        }
        public static TResult SetObjectProperties<TResult, TEntity>(this TEntity vm, TResult entity)
        {
            var type = vm.GetType();
            var properties = type.GetProperties();

            foreach (var prop in properties)
            {

                var entProp = entity.GetType().GetProperty(prop.Name);
                if (entProp != null)
                {
                    entProp.SetValue(entity, prop.GetValue(vm), null);
                }

            }

            return entity;

        }
        public static List<TResult> SetObjectPropertiesFromList<TResult, TEntity>(this List<TEntity> vmsEntities, List<TResult> entities) where TResult : new()
        {
            var properties = typeof(TEntity).GetProperties();

            foreach (var entity1 in vmsEntities)
            {
                var obj = new TResult();
                foreach (var prop in properties)
                {

                    var entProp = obj.GetType().GetProperty(prop.Name);
                    if (entProp != null)
                    {
                        entProp.SetValue(obj, prop.GetValue(entity1), null);
                    }

                }

                entities.Add(obj);
            }


            return entities;

        }
    }
}
