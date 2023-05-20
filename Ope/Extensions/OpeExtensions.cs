using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Railway
{
    public static class OpeExtensions
    {
        public const string InputParamTagKey = "InputParam";

        public static T MergeTags<T>(this T ope, Dictionary<string, string> tags) where T : OpeBase
        {
            foreach (var tag in tags)
                ope.Tags[tag.Key] = tag.Value;

            return ope;
        }

        public static T Tag<T>(this T ope, object parameter, [CallerArgumentExpression("parameter")] string parameterName = "") where T : OpeBase { ope.Tags[parameterName] = ope.Tags.ContainsKey(parameterName) ? ope.Tags[parameterName] : parameter?.FormatTagValue() ?? "NULL"; return ope; }
        public static T TagValue<T>(this T ope, string key, object val) where T : OpeBase { ope.Tags[key] = val?.FormatTagValue() ?? "NULL"; return ope; }

        private static string FormatTagValue(this object value)
        {
            Type valueType = value.GetType();

            if (valueType.IsArray)
                return string.Join(", ", ((Array)value).FormatObjectArray());

            return value.ToString();
        }

        private static IEnumerable<string> FormatObjectArray(this Array value)
        {
            var res = new List<string>();
            var enumerator = value.GetEnumerator();

            while (enumerator.MoveNext())
                res.Add(enumerator.Current.ToString());

            return res;
        }

        //internal static T Error<T>(this T ope, string userErrorMessage, Exception exception = default, string additionalStackTrace = null) where T : OpeBase
        //{
        //    ope.FeedErrorContext(userErrorMessage, exception, additionalStackTrace);

        //    return ope;
        //}

        public static Ope<TRes> Then<T, TRes>(this Ope<T> ope, Func<T, TRes> func, string userErrorMessage = null)
        {
            try
            {
                return ope.Success ? Ope.Ok(func(ope.Result)) : Ope.Error<TRes>(ope, userErrorMessage).Tag(ope.Result);
            }
            catch (Exception ex)
            {
                return Ope.Error<TRes>(ex, userErrorMessage ?? ope.UserMessage).TagValue("InputParam", ope.Result);
            }
        }
        public static Ope Then<T>(this Ope<T> ope, Action<T> func, string userErrorMessage = null)
        {
            try
            {
                if (!ope.Success)
                    return Ope.Error(ope).WithMessage(userErrorMessage ?? ope.UserMessage);

                func(ope.Result);

                return Ope.Ok();
            }
            catch (Exception ex)
            {
                return Ope.Error(ex, userErrorMessage ?? ope.UserMessage).TagValue("InputParam", ope.Result);
            }
        }

        internal static string FormatTags(this Dictionary<string, string> tags)
        {
            if (tags.Any())
            {
                return $"{Environment.NewLine}|> {string.Join($"{Environment.NewLine}|> ", tags.Select(x => $"[{x.Key}] -> [{x.Value}]"))}";
            }

            return string.Empty;
        }
    }
}
