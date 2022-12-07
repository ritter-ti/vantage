using System;

namespace Goal.Seedwork.Infra.Crosscutting.Trying
{
    public struct Try<TFailure, TSuccess>
    {
        internal TFailure Failure { get; }

        internal TSuccess Success { get; }

        public bool IsFailure { get; }

        public bool IsSuccess => !IsFailure;

        public Option<TFailure> OptionalFailure
        {
            get
            {
                if (!IsFailure)
                {
                    return Helpers.None;
                }

                return Helpers.Some(Failure);
            }
        }

        public Option<TSuccess> OptionalSuccess
        {
            get
            {
                if (!IsSuccess)
                {
                    return Helpers.None;
                }

                return Helpers.Some(Success);
            }
        }

        internal Try(TFailure failure)
        {
            IsFailure = true;
            Failure = failure;
            Success = default;
        }

        internal Try(TSuccess success)
        {
            IsFailure = false;
            Failure = default;
            Success = success;
        }

        public TResult Match<TResult>(Func<TFailure, TResult> failure, Func<TSuccess, TResult> success)
        {
            if (!IsFailure)
            {
                return success(Success);
            }

            return failure(Failure);
        }

        public Unit Match(Action<TFailure> failure, Action<TSuccess> success)
            => Match(Helpers.ToFunc(failure), Helpers.ToFunc(success));

        public TSuccess GetSuccess()
            => Success;

        public TFailure GetFailure()
            => Failure;

        public static implicit operator Try<TFailure, TSuccess>(TFailure failure)
            => new(failure);

        public static implicit operator Try<TFailure, TSuccess>(TSuccess success)
            => new(success);

        public static Try<TFailure, TSuccess> Of(TSuccess obj)
            => obj;

        public static Try<TFailure, TSuccess> Of(TFailure obj)
            => obj;
    }
}
