using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace Pl.WebFramework
{
    /// <summary>
    /// Thông tin chi tiết request khi kendo grid gửi lên server
    /// </summary>
    public class DataSourceRequest
    {
        /// <summary>
        /// Bao nhiêu bản ghi cần lấy
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Bao nhiêu bản ghi được bỏ qua
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// tập các cách sắp xếp kết qua
        /// </summary>
        public IEnumerable<Sort> Sort { get; set; }

        /// <summary>
        /// Chi tiết yêu cầu nhóm
        /// </summary>
        public IEnumerable<Group> Group { get; set; }

        /// <summary>
        /// Chi tiết tổng hợp
        /// </summary>
        public IEnumerable<Aggregator> Aggregate { get; set; }

        /// <summary>
        /// Tập các đối tượng cần lọc
        /// </summary>
        public Filter Filter { get; set; }
    }

    /// <summary>
    /// Đại diện cho một biểu thức lọc của Kendo DataSource.
    /// </summary>
    [DataContract]
    public class Filter
    {
        /// <summary>
        /// Tên trường lọc
        /// </summary>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Toán tử lọc
        /// </summary>
        [DataMember(Name = "operator")]
        public string Operator { get; set; }

        /// <summary>
        /// Giá trị lọc
        /// </summary>
        [DataMember(Name = "value")]
        public object Value { get; set; }

        /// <summary>
        /// Cách lọc
        /// </summary>
        [DataMember(Name = "logic")]
        public string Logic { get; set; }

        /// <summary>
        /// Tập hợp các phương thức lọc
        /// </summary>
        [DataMember(Name = "filters")]
        public IEnumerable<Filter> Filters { get; set; }

        /// <summary>
        /// bảng định nghĩa mapping giữa kendo filter và lingq filter
        /// </summary>
        private static readonly IDictionary<string, string> operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"doesnotcontain", "Contains"}
        };

        /// <summary>
        /// Lấy tất cả các phương thức lọc cùa filter này
        /// </summary>
        public IList<Filter> All()
        {
            List<Filter> filters = new List<Filter>();
            Collect(filters);
            return filters;
        }

        private void Collect(IList<Filter> filters)
        {
            if (Filters?.Any() == true)
            {
                foreach (Filter filter in Filters)
                {
                    filters.Add(filter);
                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }

        /// <summary>
        /// Chuyển tập hợp filter sang biểu thức lọc của lingq
        /// </summary>
        /// <param name="filters">Danh sách filter cần chuyển</param>
        public string ToExpression(IList<Filter> filters)
        {
            if (Filters?.Any() == true)
            {
                return "(" + string.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression(filters)).ToArray()) + ")";
            }

            int index = filters.IndexOf(this);
            string comparison = operators[Operator];

            if (Operator == "doesnotcontain")
            {
                return string.Format("!{0}.{1}(@{2})", Field, comparison, index);
            }

            return comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains"
                ? string.Format("{0}.{1}(@{2})", Field, comparison, index)
                : string.Format("{0} {1} @{2}", Field, comparison, index);
        }
    }

    public class GroupSelector<TElement>
    {
        public Func<TElement, object> Selector { get; set; }

        public string Field { get; set; }

        public IEnumerable<Aggregator> Aggregates { get; set; }
    }

    [DataContract(Name = "groupresult")]
    public class GroupResult
    {
        [DataMember(Name = "value")]
        public object Value { get; set; }

        public string SelectorField { get; set; }

        [DataMember(Name = "field")]
        public string Field => string.Format("{0} ({1})", SelectorField, Count);

        public int Count { get; set; }

        [DataMember(Name = "aggregates")]
        public IEnumerable<Aggregator> Aggregates { get; set; }

        [DataMember(Name = "items")]
        public dynamic Items { get; set; }

        [DataMember(Name = "hasSubgroups")]
        public bool HasSubgroups { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Value, Count);
        }
    }

    [DataContract]
    public class Sort
    {
        /// <summary>
        /// Tên trường cần xắp xếp
        /// </summary>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Lấy kiểu xắp xếp ví dụ như desc, esc
        /// </summary>
        [DataMember(Name = "dir")]
        public string Dir { get; set; }

        /// <summary>
        /// Chuyển về thành kiểu mẫu lingq
        /// </summary>
        public string ToExpression()
        {
            return Field + " " + Dir;
        }
    }

    [DataContract(Name = "aggregate")]
    public class Aggregator
    {
        /// <summary>
        /// Tên trường
        /// </summary>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Tên cách tổng hợp
        /// </summary>
        [DataMember(Name = "aggregate")]
        public string Aggregate { get; set; }

        /// <summary>
        /// Phương thức tổng hợp
        /// </summary>
        /// <param name="type">Kiểu dữ liệu của trường tổng hợp</param>
        /// <returns>Thông tin phương thức tổng hợp</returns>
        public MethodInfo MethodInfo(Type type)
        {
            Type proptype = type.GetProperty(Field).PropertyType;
            switch (Aggregate)
            {
                case "max":
                case "min":
                    return
                        GetMethod(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Aggregate),
                            MinMaxFunc().GetMethodInfo(), 2).MakeGenericMethod(type, proptype);
                case "average":
                case "sum":
                    return GetMethod(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Aggregate),
                    ((Func<Type, Type[]>)
                        GetType().GetMethod("SumAvgFunc", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(proptype).Invoke(null, null)).GetMethodInfo(), 1).MakeGenericMethod(type);

                case "count":
                    return GetMethod(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Aggregate),
                        Nullable.GetUnderlyingType(proptype) != null
                            ? CountNullableFunc().GetMethodInfo()
                            : CountFunc().GetMethodInfo(), 1).MakeGenericMethod(type);
            }
            return null;
        }

        private static MethodInfo GetMethod(string methodName, MethodInfo methodTypes, int genericArgumentsCount)
        {
            IEnumerable<MethodInfo> methods = from method in typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                                         let parameters = method.GetParameters()
                                                                         let genericArguments = method.GetGenericArguments()
                                                                         where method.Name == methodName && genericArguments.Length == genericArgumentsCount && parameters.Select(p => p.ParameterType).SequenceEqual((Type[])methodTypes.Invoke(null, genericArguments))
                                                                         select method;
            return methods.FirstOrDefault();
        }

        private static Func<Type, Type[]> CountNullableFunc()
        {
            return CountNullableDelegate;
        }

        private static Type[] CountNullableDelegate(Type t)
        {
            return new[]
            {
                typeof(IQueryable<>).MakeGenericType(t),
                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(t, typeof(bool)))
            };
        }

        private static Func<Type, Type[]> CountFunc()
        {
            return (T) => new[]
            {
                typeof(IQueryable<>).MakeGenericType(T)
            };
        }

        private static Func<Type, Type, Type[]> MinMaxFunc()
        {
            return MinMaxDelegate;
        }

        private static Type[] MinMaxDelegate(Type a, Type b)
        {
            return new[]
            {
                typeof(IQueryable<>).MakeGenericType(a),
                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(a, b))
            };
        }
    }

    public class Group : Sort
    {
        [DataMember(Name = "aggregates")]
        public IEnumerable<Aggregator> Aggregates { get; set; }
    }
}