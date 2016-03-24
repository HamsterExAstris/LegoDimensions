using System.Threading.Tasks;

namespace ShatteredTemple.LegoDimensions.Tracker.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
