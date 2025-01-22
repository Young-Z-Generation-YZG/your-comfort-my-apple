

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.BuildingBlocks.Shared.Abstractions.Result;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
}

public class Result<TResponse> : Result
{
    public TResponse? Response { get; }
    public Result(bool isSuccess, TResponse response, Error error) : base(isSuccess, error)
    {
        Response = response;
    }

    public static Result<TResponse> Success(TResponse response) => new(true, response, Error.NoneError);
    public static Result<TResponse> Failure(Error error) => new(false, default!, error);

    public static implicit operator Result<TResponse>(TResponse response) => new(true, response, Error.NoneError);
    public static implicit operator Result<TResponse>(Error error) => new(false, default!, error);
}