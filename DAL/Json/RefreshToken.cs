using System;

namespace DAL.Json;

public sealed record RefreshToken
{
    public string UserName { get; init; }

    public DateTime CreateDate { get; init; }

    public DateTime ExpireDate { get; init; }
}
