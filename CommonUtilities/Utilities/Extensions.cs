using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities.Utilities
{
    public static class Extensions
    {
        public static IEnumerable<TResult> SetObjectPropertiesFromList<TResult, TEntity>(this IEnumerable<TEntity> vmsEntities, IList<TResult> entities) where TResult : new()
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
