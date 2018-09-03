namespace Hycon.Interfaces.Domain
{
    public interface ICommand
    {
        /// <summary>
        /// Unix time offset for command timestamp
        /// </summary>
        long Timestamp { get; set; }
    }
}