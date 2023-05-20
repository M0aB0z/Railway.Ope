using System;
using System.Collections.Generic;

namespace Railway
{
    public abstract class OpeBase
    {
        public Exception Exception { get; protected set; }
        public bool Success { get; protected set; } = false;
        public Dictionary<string, string> Tags { get; protected set; } = new Dictionary<string, string>();

        public string UserMessage { get; protected set; }

        public override string ToString()
            => Success ? "SUCCESS" : GetFailedOperationDetails();

        private string GetFailedOperationDetails()
        {
            if (Exception != default)
            {
                return $"FAILURE - [{UserMessage}]{Tags.FormatTags()}{Environment.NewLine}{Exception.GetBaseException().Message}{Environment.NewLine}{Exception.StackTrace}";
            }

            return $"FAILURE - [{UserMessage}]{Environment.NewLine}{Tags.FormatTags()}";
        }
    }
}
