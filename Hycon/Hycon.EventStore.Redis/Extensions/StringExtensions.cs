using System.Text;
using Force.Crc32;

namespace Hycon.EventStore.Redis.Extensions
{
    public static class StringExtensions
    {
        public static string CalculatePartition(this string commitId)
        {
            var bytes = Encoding.UTF8.GetBytes(commitId);
            var hash = Crc32Algorithm.Compute(bytes);
            var partition = hash % 655360;

            return partition.ToString();
        }
 
    }
}