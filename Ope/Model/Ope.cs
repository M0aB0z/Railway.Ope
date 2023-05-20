using System;
using System.Threading.Tasks;

namespace Railway
{
    public class Ope : OpeBase
    {
        public static Ope Ok() => new Ope() { Success = true };
        public static Ope<T> Ok<T>(T res) => Ope<T>.Ok(res);

        public static Ope<T> Run<T>(Func<Task<T>> action, string userErrorMessage)
        {
            try
            {
                return Ok(action().Result);
            }
            catch (Exception ex)
            {
                var envStackTrace = Environment.StackTrace;
                var exception = !string.IsNullOrEmpty(envStackTrace)
                    ? new Exception(envStackTrace, ex)
                    : ex;
                return new Ope<T>().WithMessage(userErrorMessage).WithException(exception);
            }
        }

        public static Ope Error(OpeBase other) => new Ope().WithMessage(other.UserMessage).WithException(other.Exception).MergeTags(other.Tags);
        public static Ope Error(string userMessage) => new Ope().WithMessage(userMessage);
        public static Ope Error(Exception ex, string userMessage) => new Ope().WithMessage(userMessage).WithException(ex);

        public static Ope<T> Error<T>(OpeBase other, string userErrorMessage = null) => new Ope<T>()
            .WithMessage(userErrorMessage ?? other.UserMessage)
            .WithException(other.Exception).MergeTags(other.Tags);

        public static Ope<T> Error<T>(string userErrorMessage) => new Ope<T>().WithMessage(userErrorMessage);
        public static Ope<T> Error<T>(Exception exception, string userErrorMessage) => new Ope<T>().WithMessage(userErrorMessage).WithException(exception);
        public static Ope<T> Error<T>(T res, string userErrorMessage) => new Ope<T>().WithMessage(userErrorMessage).WithResult(res);
        public static Ope<T> Error<T>(T res, Exception exception, string userErrorMessage) => new Ope<T>().WithMessage(userErrorMessage).WithException(exception).WithResult(res);
        public static Ope<T> Error<T>(T res, OpeBase other, string userErrorMessage = null) => new Ope<T>().WithMessage(userErrorMessage ?? other.UserMessage).WithException(other.Exception).MergeTags(other.Tags).WithResult(res);

        public Ope WithMessage(string userMessage) { UserMessage = userMessage; return this; }
        internal Ope WithException(Exception exception) { Exception = exception; return this; }
    }

    public class Ope<T> : OpeBase
    {
        public T Result { get; private set; }
        internal static Ope<T> Ok(T result) => new Ope<T>() { Success = true, Result = result };

        internal Ope<T> WithResult(T res) { Result = res; return this; }
        public Ope<T> WithMessage(string userMessage) { UserMessage = userMessage; return this; }
        internal Ope<T> WithException(Exception exception) { Exception = exception; return this; }
    }
}
