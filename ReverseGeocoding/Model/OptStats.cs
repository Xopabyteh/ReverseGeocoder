namespace ReverseGeocoding.Model;

public readonly ref struct OptStats
{
    public long RecordsIn { get; }
    public long RecordsOut { get; }

    public OptStats(long @in, long @out)
    {
        RecordsIn = @in;
        RecordsOut = @out;
    }
}