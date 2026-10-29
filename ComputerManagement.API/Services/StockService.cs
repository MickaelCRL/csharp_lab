namespace ComputerManagement.API.Services;

public class StockService
{
    public int SsdStock { get; private set; } = 10;
    private readonly System.Threading.Lock _lock = new();

    public void ConsumeSsd()
    {
        lock (_lock)
        {
            var currentStock = SsdStock;
            Task.Delay(10).Wait();
            SsdStock = currentStock - 1;
        }
    }
}