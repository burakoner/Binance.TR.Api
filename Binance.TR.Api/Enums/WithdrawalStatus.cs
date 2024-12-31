namespace Binance.TR.Api.Enums;

public enum WithdrawalStatus
{
    [Map("0")]
    EmailSent = 0,

    [Map("1")]
    Cancelled = 1,

    [Map("2")]
    AwaitingApproval = 2,

    [Map("3")]
    Rejected = 3,

    [Map("4")]
    Processing = 4,

    [Map("5")]
    Failure = 5,

    [Map("10")]
    Completed = 10,
}