

long timestamp = 1617282420;


DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(timestamp);

DateTime unixEpoch = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc);

DateTime dateFromUnixTimestamp = unixEpoch.AddSeconds(timestamp);

long dateAsTimestamp = new DateTimeOffset(dateFromUnixTimestamp)
    .ToUnixTimeSeconds();

Console.ReadLine();