using System.Collections.Concurrent;

namespace BookStore.Web.Middleware;

// singleton – bezpieczny wielowątkowo licznik
public class RequestStats
{
    private long _total;
    // Zadanie 2: ścieżka → liczba żądań
    private readonly ConcurrentDictionary<string, long> _paths = new();

    // odczyt stanu licznika
    public long Total => Interlocked.Read(ref _total);

    // inkrementacja licznika
    public long Hit(string path)
    {
        _paths.AddOrUpdate(path, 1, (_, n) => n + 1);
        return Interlocked.Increment(ref _total);
    }

    // Zadanie 2: najpopularniejsze ścieżki
    public IEnumerable<KeyValuePair<string, long>> Top(int count) =>
        _paths.OrderByDescending(p => p.Value).Take(count);
}
